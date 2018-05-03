IF OBJECT_ID('dbo.db_Purge_ByDate') IS NOT NULL
BEGIN
	PRINT 'Dropping dbo.db_Purge_ByDate'
	DROP PROCEDURE dbo.db_Purge_ByDate
	-- Tell user if drop failed.
	IF OBJECT_ID('dbo.db_Purge_ByDate') IS NOT NULL
		PRINT '***********Drop of dbo.db_Purge_ByDate FAILED.'
END
GO
PRINT 'Creating dbo.db_Purge_ByDate'
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO



CREATE PROCEDURE [dbo].[db_Purge_ByDate]
/*
* PARAMETERS:
*	@PurgeDate - Date that is passed in to purge based off of.
*
* DESCRIPTION:
*	Purges the database of all estimates and unused rates that exist prior to the given date
*
* TABLES:
*   Table Name          Access
*   ==========          ======
*   MANY                DELETE
*
* PROCEDURES CALLED:
*	None
*
* DATABASE:
*	All
*
* RETURN VALUE:
*	None 
*
* REVISION HISTORY:
*
* Date            Who             Comments
* -------------   --------        -------------------------------------------------
* 07/10/2007      NLS             Initial Creation
* 09/12/2007	  NLS			  Renamed proc and made changes to tables affected 
* 09/26/2007      NLS             Added Report purging
* 10/05/2007      NLS             Change order on estimate delete so children purged before parents
* 10/05/2007      NLS             Fixed up Publication Rate Purge
* 02/05/2009      TAP				Fixed Line 316 Added Est_Package_ID is Null - Cleaned up ALiases
*									in delete statements from pub
*									fixed join on pub_dayofweek rates join to keep from deleting all records
* 11/17/2015	  VM			  Commented the delete statements of newspaper rates tables
*/

@PurgeDate datetime

AS
BEGIN
	-- Used when performing some transactional processing to determine whether to rollback
	-- a transaction based on a partial failure on a delete
	DECLARE @ErrorFlag bit

	SET @ErrorFlag = 0

	BEGIN TRAN Purge

		CREATE TABLE #Est_Estimate (est_estimate_id bigint)	
		CREATE TABLE #pub_pubrate_map (pub_pubrate_map_id bigint)
		
		----------------------------------------------------------------------------------------------
		-- Find all estimates that need to be purged based on the RunDate
		-- Be sure to delete the estimates that have a parent first
		----------------------------------------------------------------------------------------------
		BEGIN
			INSERT INTO #Est_Estimate
			SELECT est_estimate_id FROM Est_Estimate WHERE RunDate <= @PurgeDate
			ORDER BY isnull(parent_id, -1) DESC

				-- Delete from the tables where there are no dependencies except on est_estimate_id
			DELETE samp FROM est_samples AS samp INNER JOIN #Est_Estimate AS est ON est.est_estimate_id = samp.est_estimate_id
			DELETE assem FROM est_assemdistriboptions AS assem INNER JOIN #Est_Estimate AS est ON est.est_estimate_id = assem.est_estimate_id
			DELETE pub FROM est_pubissuedates AS pub INNER JOIN #Est_Estimate AS est ON est.est_estimate_id = pub.est_estimate_id

			DELETE map FROM est_packagepolybag_map AS map INNER JOIN Est_Package AS pack ON pack.est_package_id = map.est_package_id INNER JOIN #Est_Estimate AS est ON est.est_estimate_id = pack.est_estimate_id
			DELETE packmap FROM est_packagecomponentmapping AS packmap INNER JOIN Est_Package AS pack ON pack.est_package_id = packmap.est_package_id INNER JOIN #Est_Estimate AS est ON est.est_estimate_id = pack.est_estimate_id

			-- Delete the Component Dependencies
			DELETE pcm FROM est_packagecomponentmapping AS pcm
				LEFT JOIN est_component c ON pcm.est_component_id = pcm.est_component_id
			INNER JOIN #Est_Estimate AS est ON est.est_estimate_id = c.est_estimate_id
			
			-- Now delete the packages, components and polybag maps now that the dependencies are gone
			DELETE pack FROM est_package AS pack INNER JOIN #Est_Estimate AS est ON est.est_estimate_id = pack.est_estimate_id
			DELETE comp FROM est_component AS comp INNER JOIN #Est_Estimate AS est ON est.est_estimate_id = comp.est_estimate_id
			DELETE estmap FROM est_estimatepolybaggroup_map AS estmap INNER JOIN #Est_Estimate AS est ON est.est_estimate_id = estmap.est_estimate_id
			
			-- Finally delete the estimate
			-- Deleting an estimate could fail if all it's children weren't deleted
			-- in this case, rollback the estimate delete because you can't delete a parent
			-- if any children didn't meet the purge criteria
			DELETE estim FROM est_estimate AS estim INNER JOIN #Est_Estimate AS est ON est.est_estimate_id = estim.est_estimate_id
			
			IF (@@ERROR <> 0) 
			BEGIN
				SET @ErrorFlag = 1
			END		
		END 
			

		----------------------------------------------------------------------------------------------
		-- Now delete any Polybag Groups that are empty as a result of the estimate purge
		----------------------------------------------------------------------------------------------
		IF @ErrorFlag = 0
		BEGIN
			DELETE polybag FROM est_polybag AS polybag INNER JOIN (SELECT pbg.est_polybaggroup_id FROM Est_PolybagGroup pbg
				LEFT JOIN est_estimatepolybaggroup_map map ON pbg.est_polybaggroup_id = map.est_polybaggroup_id
			WHERE map.est_polybaggroup_id is null) AS polybaggroup ON polybaggroup.est_polybaggroup_id = polybag.est_polybaggroup_id
			
			DELETE polybaggrp FROM est_polybaggroup AS polybaggrp INNER JOIN (SELECT pbg.est_polybaggroup_id FROM Est_PolybagGroup pbg
				LEFT JOIN est_estimatepolybaggroup_map map ON pbg.est_polybaggroup_id = map.est_polybaggroup_id
			WHERE map.est_polybaggroup_id is null) AS polybaggroup ON polybaggroup.est_polybaggroup_id = polybaggrp.est_polybaggroup_id
			
			IF (@@ERROR <> 0) 
			BEGIN
				SET @ErrorFlag = 1
			END				
		END

		----------------------------------------------------------------------------------------------
		-- Now find all rates that are effective before the purge date, but are not used by an estimate
		----------------------------------------------------------------------------------------------
		IF @ErrorFlag = 0
		BEGIN
			-- Mailhouse Rates
			DELETE mhr FROM vnd_mailhouserate mhr
				LEFT JOIN est_assemdistriboptions ado ON mhr.vnd_mailhouserate_id = ado.mailhouse_id
			WHERE ado.mailhouse_id is null AND mhr.effectivedate <= @PurgeDate
			
			IF (@@ERROR <> 0) 
			BEGIN
				SET @ErrorFlag = 1
			END				
		END
				
		-- Paper Rates
		-- Attempt to delete the Paper Map and Paper Record.  If there is any failure, then rollback
		-- the entire tran for this rate	
		IF @ErrorFlag = 0
		BEGIN
			DELETE papermap FROM ppr_paper_map AS papermap
			INNER JOIN
			(SELECT vnd_paper_id FROM vnd_paper WHERE effectivedate <= @PurgeDate) AS VND		
			ON
				VND.vnd_paper_id = papermap.vnd_paper_id
			INNER JOIN
				est_component AS comp ON comp.paper_map_id = papermap.ppr_paper_map_id 				
			INNER JOIN 
				#Est_Estimate AS est ON est.est_estimate_id = comp.est_estimate_id
			
			IF (@@ERROR <> 0) 
			BEGIN
				SET @ErrorFlag = 1
			END				
			
			IF (@ErrorFlag = 0) BEGIN
				DELETE vnd FROM vnd_paper AS vnd 
				INNER JOIN 
					ppr_paper_map AS papermap ON papermap.vnd_paper_id = vnd.vnd_paper_id
				INNER JOIN
				(SELECT vnd_paper_id FROM vnd_paper WHERE effectivedate <= @PurgeDate) AS VND		
				ON
					VND.vnd_paper_id = papermap.vnd_paper_id
				INNER JOIN
					est_component AS comp ON comp.paper_map_id = papermap.ppr_paper_map_id 				
				INNER JOIN 
					#Est_Estimate AS est ON est.est_estimate_id = comp.est_estimate_id				
				WHERE vnd.effectivedate <= @PurgeDate
				
				IF (@@ERROR <> 0) BEGIN
					SET @ErrorFlag = 1
				END
			END
			IF (@@ERROR <> 0) 
			BEGIN
				SET @ErrorFlag = 1
			END				
		END		
		
		IF (@ErrorFlag = 0) 
		BEGIN
			-- Mail Tracking Rates
			DELETE mtr FROM vnd_mailtrackingrate mtr
				LEFT JOIN est_assemdistriboptions ado ON mtr.vnd_mailtrackingrate_id = ado.mailtracking_id
			WHERE ado.mailtracking_id is null AND mtr.effectivedate <= @PurgeDate
			
			IF (@@ERROR <> 0) 
			BEGIN
				SET @ErrorFlag = 1
			END				
		END
		
		IF (@ErrorFlag = 0) 
		BEGIN
			-- Maillist Resource Rates
			DELETE mlr FROM vnd_maillistresourcerate mlr
				LEFT JOIN est_assemdistriboptions ado ON mlr.vnd_maillistresourcerate_id = ado.maillistresource_id
			WHERE ado.maillistresource_id is null AND mlr.effectivedate <= @PurgeDate
			
			IF (@@ERROR <> 0) 
			BEGIN
				SET @ErrorFlag = 1
			END				
		END

		-- Printer Rates
		IF (@ErrorFlag = 0) 
		BEGIN
			-- Attempt to delete the Printer Rate and Paper Record.  If there is any failure, then rollback
			-- the entire tran for this rate
			DELETE rate FROM prt_printerrate rate
			INNER JOIN
			(SELECT vnd_printer_id FROM vnd_printer WHERE effectivedate <= @PurgeDate) AS printer
			ON printer.vnd_printer_id = rate.vnd_printer_id
			INNER JOIN
				est_component AS comp ON comp.platecost_id = rate.prt_printerrate_id 				
			INNER JOIN 
				#Est_Estimate AS est ON est.est_estimate_id = comp.est_estimate_id				

			IF (@@ERROR <> 0) BEGIN
				SET @ErrorFlag = 1
			END

			IF (@ErrorFlag = 0) BEGIN
				DELETE vnd FROM vnd_printer AS vnd
				INNER JOIN
					prt_printerrate rate ON rate.vnd_printer_id = vnd.vnd_printer_id
				INNER JOIN
					est_component AS comp ON comp.digitalhandlenprepare_id = rate.prt_printerrate_id 				
				INNER JOIN 
					#Est_Estimate AS est ON est.est_estimate_id = comp.est_estimate_id						
				WHERE vnd.effectivedate <= @PurgeDate
				IF (@@ERROR <> 0) BEGIN
					SET @ErrorFlag = 1
				END
			END
		END
		
		----------------------------------------------------------------------------------------------
		-- Postal Scenarios that aren't used by polybags and meet the Purge Date criteria
		----------------------------------------------------------------------------------------------
		IF (@ErrorFlag = 0) 
		BEGIN	
			DELETE map FROM pst_postalcategoryscenario_map map 
			INNER JOIN
			(SELECT pst.pst_postalscenario_id FROM pst_postalscenario pst
					LEFT JOIN est_polybag pb ON pst.pst_postalscenario_id = pb.pst_postalscenario_id
				WHERE pb.pst_postalscenario_id is null AND pst.effectivedate <= @PurgeDate) AS scenario
			ON 
				scenario.pst_postalscenario_id = map.pst_postalscenario_id
			IF (@@ERROR <> 0) BEGIN
				SET @ErrorFlag = 1
			END			
			
			IF (@ErrorFlag = 0) 
			BEGIN		
				DELETE pst FROM pst_postalscenario pst
						LEFT JOIN est_polybag pb ON pst.pst_postalscenario_id = pb.pst_postalscenario_id
				INNER JOIN
					est_assemdistriboptions AS assem ON assem.pst_postalscenario_id = pst.pst_postalscenario_id
				INNER JOIN 
					#Est_Estimate AS est ON est.est_estimate_id = assem.est_estimate_id										
				WHERE pb.pst_postalscenario_id is null AND pst.effectivedate <= @PurgeDate	
				IF (@@ERROR <> 0) BEGIN
					SET @ErrorFlag = 1
				END			
			END
		END	
		
		-- Postal Rates that aren't being used by a postal scenario and also meet the date criteria
		IF (@ErrorFlag = 0) 
		BEGIN		
			-- Attempt to delete the Postal Rate Records.  If there is any failure, then rollback
			-- the entire tran for this rate.  Might fail because it's linked to a scenario which is still active.
			DELETE map FROM pst_postalcategoryrate_map map
			INNER JOIN (SELECT pst_postalweights_id FROM pst_postalweights WHERE effectivedate <= @PurgeDate) AS weights
			ON weights.pst_postalweights_id = map.pst_postalweights_id
			INNER JOIN pst_postalcategoryscenario_map categorymap ON  categorymap.pst_postalcategoryrate_map_id = map.pst_postalcategoryrate_map_id
			INNER JOIN
			(SELECT pst.pst_postalscenario_id FROM pst_postalscenario pst
					LEFT JOIN est_polybag pb ON pst.pst_postalscenario_id = pb.pst_postalscenario_id
				WHERE pb.pst_postalscenario_id is null AND pst.effectivedate <= @PurgeDate) AS scenario
			ON 
				scenario.pst_postalscenario_id = categorymap.pst_postalscenario_id
				
			IF (@@ERROR <> 0) BEGIN
				SET @ErrorFlag = 1
			END
			
			IF (@ErrorFlag = 0) BEGIN
				DELETE pst FROM pst_postalweights AS pst 
				INNER JOIN pst_postalcategoryrate_map map ON map.pst_postalweights_id = pst.pst_postalweights_id
				INNER JOIN pst_postalcategoryscenario_map categorymap ON  categorymap.pst_postalcategoryrate_map_id = map.pst_postalcategoryrate_map_id
				INNER JOIN
				(SELECT pst.pst_postalscenario_id FROM pst_postalscenario pst
						LEFT JOIN est_polybag pb ON pst.pst_postalscenario_id = pb.pst_postalscenario_id
					WHERE pb.pst_postalscenario_id is null AND pst.effectivedate <= @PurgeDate) AS scenario
				ON 
					scenario.pst_postalscenario_id = categorymap.pst_postalscenario_id
				WHERE pst.effectivedate <= @PurgeDate
				IF (@@ERROR <> 0) BEGIN
					SET @ErrorFlag = 1
				END
			END
		END	
		
		----------------------------------------------------------------------------------------------
		-- Publication Rates
		----------------------------------------------------------------------------------------------
		-- First Purge pub_pubgroups and map tables
		IF (@ErrorFlag = 0) 
		BEGIN		
			DELETE map FROM pub_pubpubgroup_map AS map
			INNER JOIN
			(SELECT pg.pub_pubgroup_id FROM pub_pubgroup pg
				LEFT JOIN est_package pkg ON pg.pub_pubgroup_id = pkg.pub_pubgroup_id
			WHERE effectivedate <= @PurgeDate) AS pubgroup
			ON
				pubgroup.pub_pubgroup_id = map.pub_pubgroup_id		
			IF (@@ERROR <> 0) BEGIN
				SET @ErrorFlag = 1
			END		
		END		

		IF (@ErrorFlag = 0) 
		BEGIN		
			DELETE pg FROM pub_pubgroup pg
			INNER JOIN est_package AS pack ON pack.pub_pubgroup_id = pg.pub_pubgroup_id
			INNER JOIN #Est_Estimate AS est ON est.est_estimate_id = pack.est_estimate_id				
			LEFT JOIN pub_pubpubgroup_map pgmap ON pg.pub_pubgroup_id = pgmap.pub_pubgroup_id			
			WHERE pgmap.pub_pubgroup_id is null
			IF (@@ERROR <> 0) BEGIN
				SET @ErrorFlag = 1
			END		
		END

		IF (@ErrorFlag = 0) 
		BEGIN		
			DELETE gis FROM pub_groupinsertscenario_map gis
				LEFT JOIN pub_pubgroup pg ON pg.description = gis.pubgroupdescription
			WHERE pg.description is null
			IF (@@ERROR <> 0) BEGIN
				SET @ErrorFlag = 1
			END		
		END
		
		IF (@ErrorFlag = 0) 
		BEGIN		
			DELETE pis FROM pub_insertscenario pis
				LEFT JOIN pub_groupinsertscenario_map gis ON pis.pub_insertscenario_id = gis.pub_insertscenario_id
			WHERE gis.pub_insertscenario_id is null
			IF (@@ERROR <> 0) BEGIN
				SET @ErrorFlag = 1
			END		
		END
		
		IF (@ErrorFlag = 0) 
		BEGIN		
			-- Delete Pub Rates and Quantities for any pubrate maps where there are no groups anymore
			INSERT INTO #pub_pubrate_map
			SELECT rmap.pub_pubrate_map_id FROM pub_pubrate_map rmap
				LEFT JOIN pub_pubpubgroup_map gmap ON rmap.pub_pubrate_map_id = gmap.pub_pubrate_map_id
			WHERE gmap.pub_pubrate_map_id is null
						
			-- DELETE pubquantities and associated tables
			DELETE dow FROM pub_dayofweekquantity dow
				JOIN pub_pubquantity q ON dow.pub_pubquantity_id = q.pub_pubquantity_id
				JOIN pub_pubrate_map map ON q.pub_pubrate_map_id = map.pub_pubrate_map_id
				JOIN #pub_pubrate_map pubrate ON pubrate.pub_pubrate_map_id = map.pub_pubrate_map_id
			WHERE
				q.effectivedate <= @PurgeDate 

			DELETE q FROM pub_pubquantity q
				JOIN pub_pubrate_map map ON q.pub_pubrate_map_id = map.pub_pubrate_map_id
				JOIN #pub_pubrate_map pubrate ON pubrate.pub_pubrate_map_id = map.pub_pubrate_map_id					
			WHERE
				q.effectivedate <= @PurgeDate 
		    			
		
			---- DELETE pubrates and associated tables
			--DELETE dow FROM pub_dayofweekrates dow
			--    JOIN pub_dayofweekratetypes dowtype ON dow.pub_dayofweekratetypes_id = dowtype.pub_dayofweekratetypes_id
			--    JOIN pub_pubrate rate ON dowtype.pub_pubrate_id = dowtype.pub_pubrate_id
			--    JOIN pub_pubrate_map map ON rate.pub_pubrate_map_id = map.pub_pubrate_map_id
				--JOIN #pub_pubrate_map pubrate ON pubrate.pub_pubrate_map_id = map.pub_pubrate_map_id								
			--WHERE
			--    rate.effectivedate <= @PurgeDate 
		    
			--DELETE dowtype FROM pub_dayofweekratetypes dowtype 
			--    JOIN pub_pubrate rate ON dowtype.pub_pubrate_id = dowtype.pub_pubrate_id
			--    JOIN pub_pubrate_map map ON rate.pub_pubrate_map_id = map.pub_pubrate_map_id
				--JOIN #pub_pubrate_map pubrate ON pubrate.pub_pubrate_map_id = map.pub_pubrate_map_id											
			--WHERE
			--    rate.effectivedate <= @PurgeDate 
		    
			--DELETE rate FROM pub_pubrate rate
			--    JOIN pub_pubrate_map map ON rate.pub_pubrate_map_id = map.pub_pubrate_map_id
				--JOIN #pub_pubrate_map pubrate ON pubrate.pub_pubrate_map_id = map.pub_pubrate_map_id											
			--WHERE
			--    rate.effectivedate <= @PurgeDate 
			IF (@@ERROR <> 0) BEGIN
				SET @ErrorFlag = 1
			END							
		END		    
			-- DELETE pubquantities and associated tables
			---- Delete Activate records pub rate maps where there are no more rates despite effective date
			--DELETE activate FROM pub_pubrate_map_activate AS activate
			--    JOIN pub_pubrate_map map ON activate.pub_pubrate_map_id = map.pub_pubrate_map_id
			--    LEFT JOIN pub_pubrate r ON map.pub_pubrate_map_id = r.pub_pubrate_map_id
			--    LEFT JOIN pub_pubquantity q ON map.pub_pubrate_map_id = q.pub_pubrate_map_id
			--    LEFT JOIN pub_pubgroup g ON map.pub_pubrate_map_id = g.pub_pubrate_map_id
			--WHERE
			--    r.pub_pubrate_map_id is null AND
			--    q.pub_pubrate_map_id is null AND
			--    g.pub_pubrate_map_id is null

			---- Delete All Orphaned pub rate maps
			--DELETE map FROM pub_pubrate_map AS map
			--    LEFT JOIN pub_pubrate_map_activate a ON map.pub_pubrate_map_id = a.pub_pubrate_map_id
			--    LEFT JOIN pub_pubrate r ON map.pub_pubrate_map_id = r.pub_pubrate_map_id
			--    LEFT JOIN pub_pubquantity q ON map.pub_pubrate_map_id = q.pub_pubrate_map_id
			--    LEFT JOIN pub_pubgroup g ON map.pub_pubrate_map_id = g.pub_pubrate_map_id
			--WHERE
			--    a.pub_pubrate_map_id is null AND
			--    r.pub_pubrate_map_id is null AND
			--    q.pub_pubrate_map_id is null AND
			--    g.pub_pubrate_map_id is null

		----------------------------------------------------------------------------------------------
		-- Reports based on the date the report was run
		----------------------------------------------------------------------------------------------
		IF (@ErrorFlag = 0) 
		BEGIN		
			DELETE FROM rpt_report WHERE createddate <= @PurgeDate
			IF (@@ERROR <> 0) BEGIN
				SET @ErrorFlag = 1
			END					
		END
		
		DROP TABLE #Est_Estimate
		DROP TABLE #pub_pubrate_map
				
		-- Commit the entire transaction		
		IF (@ErrorFlag = 0) 
		BEGIN
			COMMIT TRAN Purge
		END				
		-- Rollback the entire transaction
		IF (@ErrorFlag = 1) 
		BEGIN
			ROLLBACK TRAN Purge
		END				

END

GO

GRANT  EXECUTE  ON [dbo].[db_Purge_ByDate]  TO [PMES_SuperAdmin], [PMES_RateAdmin], [PMES_Create], [PMES_ReadOnly]
GO