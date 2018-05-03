Imports TurnInProcessAutomation.BusinessEntities
Imports System.Configuration
Imports System.Collections.Generic
Imports System.Reflection
Imports System.Text
Imports System.IO
Imports System.Xml.Serialization
Imports IBM.Data.DB2
Imports Microsoft.VisualBasic.FileIO
Imports System.Data.SqlClient
Imports System.Web
Imports System.Xml
Imports System.Net


Module Module1
    Dim R As New Random
    Dim CSVPullfolder As String = "C:\test.csv"
    Dim XMLFromWHPushfolder As String = "C:\temp\MerchandiseSchema\FromWorkhorse\"
    Dim XMLToWHPushfolder As String = "C:\temp\MerchandiseSchema\ToWorkhorse\"
    Dim ComparePath As String = "C:\temp\CompareCMR\"

    Sub Main()


        'Dim x As String = "19843670066"
        'Console.WriteLine(x.Substring(x.IndexOf("-"c) + 1) & " :: " & x)
        'Dim x As String = "<MerchandiseSample xmlns='http://workhorse.qa.bonton.com/Schema/BonTon_WhMessage'><ApprovalFlag>0</ApprovalFlag><ApprovalStatus>T</ApprovalStatus><BoxNumber>1</BoxNumber><BrandDesc>Home Interior</BrandDesc><BrandId>0</BrandId><BuyerDesc /><BuyerGroupEmail>BY50401@Bonton.com</BuyerGroupEmail><BuyerId>40</BuyerId><ClassDesc>Dummy Class</ClassDesc><ClassId>50</ClassId><CMGDesc>MODERATE SPORTSWEAR</CMGDesc><CMGID>130</CMGID><CMRNotes>WH</CMRNotes><ColorCode>903</ColorCode><ColorDesc>CML/G</ColorDesc><CRGDesc>READY TO WEAR</CRGDesc><CRGID>100</CRGID><DeptDesc>MS RELATIVITY SPTS            </DeptDesc><DeptID>154</DeptID><Disposition>WH</Disposition><ExpediteReturn>1</ExpediteReturn><ISN>154101070</ISN><ISNChanged>0</ISNChanged><ISNDescription>SPCDY CREWNK W/ RIB INSETS</ISNDescription><IsWebEligible>1</IsWebEligible><LabelDesc /><LabelID>0</LabelID><MerchID>100080</MerchID><Quantity>1</Quantity><Requestor>abarde</Requestor><ReturnAddress1>WH</ReturnAddress1><ReturnAddress2>WH</ReturnAddress2><ReturnCity>WH</ReturnCity><ReturnDate>2015-02-02T14:07:52.53321</ReturnDate><ReturnPhone>WH</ReturnPhone><ReturnState>WH</ReturnState><ReturnZip>WH</ReturnZip><SampleCareNotes>Test Sample Care Notes</SampleCareNotes><SampleDueInhouseDate>2015-02-02T14:07:52.53321</SampleDueInhouseDate><SamplePrimaryLocation>Milwaukee-CMR</SamplePrimaryLocation><SampleRequestedDate>2015-02-02T14:07:52.53321</SampleRequestedDate><SampleRequestorEmail /><SampleRequestorName /><SampleRequestType>Sample</SampleRequestType><SampleSecondaryLocation>WH</SampleSecondaryLocation><SampleSource>StyleSKU</SampleSource><SampleStatus>Received</SampleStatus><SellingLocation>0</SellingLocation><SequenceNumber>1</SequenceNumber><ShelfNumber>1</ShelfNumber><SizeCode>4109</SizeCode><SizeDesc>Med</SizeDesc><TurninMerchandiseID>0</TurninMerchandiseID><UPC>2235501</UPC><User>abarde</User><VendorEmail>dummy@dummy.com</VendorEmail><VendorId>34425</VendorId><VendorInstructions>OT</VendorInstructions><VendorName>TLB HOLDINGS INC/PMG</VendorName><VendorPhone>2624495558</VendorPhone><VendorPID>RE3FA027M</VendorPID><VendorStyleNumber>RE3FA027M</VendorStyleNumber><SnapshotImage><FileName /><PrimaryView><ActualURL /><MediumURL /><ThumbnailURL /></PrimaryView><SecondaryView><ActualURL /><MediumURL /><ThumbnailURL /></SecondaryView></SnapshotImage></MerchandiseSample>"
        'Dim m As New MerchandiseSample
        'm = Helper.Deserialize(x)
        'Console.Write(m.MerchID)

        'MerchCreate, MerchUpdate, MerchGet, JobCreateOrUpdate

        '------ BEGIN : Testing *** MerchCreate  *** Call for WorkHorse -----
        'Dim ISN As Integer = 61100126
        'DeleteCompareFIles()
        'SendNewMerchandiseToWorkhorse(ISN)
        '------ BEGIN : Testing MerchCreate Call for WorkHorse -----

        '------ BEGIN : Testing ***  GetMerch  *** Call for WorkHorse -----
        'Dim m As New MerchandiseSample
        'm.MerchID = 100127
        'GetMerchandiseFromWH(m)
        '------ BEGIN : Testing GetMerch Call for WorkHorse -----

        '------ BEGIN : Testing ***  MerchUpdate  *** Call for WorkHorse -----
        'Dim m As New MerchandiseSample
        'm.MerchID = 854534
        'GetMerchandiseFromWH(m)
        ''m.SampleStatus = "Returned"
        'UpdateMerchandiseOnWH(m)

        'UpdateMerchandiseOnWHwithString("")

        '------ BEGIN : Testing GetMerch Call for WorkHorse -----

        'Update Request
        'Dim WHRequest As String = CreateWorkhorseXML(GenerateMerchandise(merchId, Operation.MerchUpdate), Operation.MerchUpdate)
        'SendToWorkHorse(WHRequest)

        '' ----------- BEGIN : Generate new merchandise XML Schema from an ISN ----
        'Dim ISN As Integer = 61100126
        'GenerateNewMerchandiseXMLToFolder(ISN)
        'GenerateNewMerchandiseXMLToQueue(ISN)
        '' ----------- END : Generate XML Schema from a merchId ----

        

        '' ----------- BEGIN : Generate XML Schema from a merchId ----
        'Dim merchId As Integer = 800001
        'GenerateMerchandiseXMLToFolder(merchId)
        '' ----------- END : Generate XML Schema from a merchId ----

        ' Dim DebuggingXML As String = "<MerchandiseSample xmlns='http://workhorse.qa.bonton.com/Schema/BonTon_WhMessage'><ApprovalFlag>false</ApprovalFlag> <BoxNumber>0</BoxNumber> <BrandDesc>Baby Essentials</BrandDesc> <BrandId>87</BrandId> <BuyerDesc>Infants/Newborn</BuyerDesc> <BuyerGroupEmail>BY53701@Bonton.com</BuyerGroupEmail> <BuyerId>370</BuyerId> <ClassDesc>AD SUTTON</ClassDesc> <ClassId>409</ClassId> <CMGDesc>CHILDRENS</CMGDesc> <CMGID>509</CMGID> <ColorCode>100</ColorCode> <ColorDesc>WHITE</ColorDesc> <CRGDesc>CHILDRENS</CRGDesc> <CRGID>300</CRGID> <DeptDesc>NEWBORN 0-9 MONTHS</DeptDesc> <DeptID>43</DeptID> <Disposition>6017</Disposition> <ExpediteReturn>false</ExpediteReturn> <ISN>43105920</ISN> <ISNChanged>false</ISNChanged> <ISNDescription>NBG BLK/WHT FLRL ROMPER</ISNDescription> <IsWebEligible>true</IsWebEligible> <LabelDesc>Baby Essentials</LabelDesc> <LabelID>87</LabelID> <LastUsedDate>2015-03-11T14:09:13</LastUsedDate> <MerchID>800042</MerchID> <PlaceholderAdNumber>17476</PlaceholderAdNumber> <PlaceholderPageNumber>3</PlaceholderPageNumber> <Quantity>1</Quantity> <Requestor>SR/vkollipara</Requestor> <SampleDueInhouseDate>2015-04-24T00:00:00</SampleDueInhouseDate> <SamplePrimaryLocation>955-CORP AD SMPL</SamplePrimaryLocation> <SampleRequestType>SAMPLE</SampleRequestType> <SampleSource>Vendor Sample</SampleSource> <SampleStatus>Requested</SampleStatus> <SellingLocation>false</SellingLocation> <SequenceNumber>0</SequenceNumber> <ShelfNumber>04 00 00</ShelfNumber> <SizeCode>644</SizeCode> <SizeDesc>6MO</SizeDesc> <TurninMerchandiseID>0</TurninMerchandiseID> <UPC>886252135635</UPC> <User>mpeters</User> <VendorEmail>Vinaya1234@bonton.com,vinaya1@bonton.com</VendorEmail> <VendorId>1271</VendorId> <VendorName>AD SUTTON &amp; SONS</VendorName> <VendorPID>88077</VendorPID> <VendorStyleNumber>88077</VendorStyleNumber> <SnapshotImage> <PrimaryView/> <SecondaryView/> </SnapshotImage> </MerchandiseSample>"
        'DebuggingXML += ""
        'Dim m As MerchandiseSample = DebugMerchandise(DebuggingXML)
        'ValidateMerchandiseForTTU450(m)
        'Dim ad As Integer = 4918
        'Dim page As Integer = 10
        'Dim WorkhorseXML As String
        'WorkhorseXML = "<?xml version='1.0' encoding='UTF-8'?><WorkhorseRequest xmlns:out='http://workhorse.qa.bonton.com/Schema/BonTon_WhMessage' xmlns='http://workhorse.qa.bonton.com/Schema/BonTon_WhMessage' xmlns:xsi='http://www.w3.org/2001/XMLSchema-instance' xsi:schemaLocation='http://workhorse.qa.bonton.com/Schema/BonTon_WhMessage http://workhorse.qa.bonton.com/Schema/BonTon_WhMessage.xsd'>"
        'WorkhorseXML += "<RequestHeader>"
        'WorkhorseXML += "<SecurityInfo>"
        'WorkhorseXML += "	<workhorseLogin>msg</workhorseLogin>"
        'WorkhorseXML += "	<encryptedPassword>341A10747C4533634CA4F0588767AECDB39F2F5B5CD77C6640258CB8D47DF149</encryptedPassword>"
        'WorkhorseXML += "	<mutualToken>BonTon</mutualToken>"
        'WorkhorseXML += "</SecurityInfo>"
        'WorkhorseXML += "<Operation>JobCreateOrUpdate</Operation>"
        'WorkhorseXML += "</RequestHeader>"
        'WorkhorseXML += "<RequestDetail>"
        'WorkhorseXML += "<JobInfo>"
        'WorkhorseXML += "<Ad>"
        'WorkhorseXML += "<AdNumber>" + ad.ToString + "</AdNumber>"
        'WorkhorseXML += "<Page>"
        'WorkhorseXML += "		<PageNumber>" + page.ToString + "</PageNumber>"
        'WorkhorseXML += "		<ActiveIndicator>A</ActiveIndicator>"
        'WorkhorseXML += "		<ShotGroup>"
        'WorkhorseXML += "			<ShotNumber>0</ShotNumber>"
        'WorkhorseXML += "			<Image>"
        'WorkhorseXML += "			<ImageNumber>696731</ImageNumber>"
        'WorkhorseXML += "			<ActiveIndicator>A</ActiveIndicator>"
        'WorkhorseXML += "			<MerchForImage>"
        'WorkhorseXML += "				<MerchID>800360</MerchID>"
        'WorkhorseXML += "				<ActiveIndicator>A</ActiveIndicator>"
        'WorkhorseXML += "			</MerchForImage>"
        'WorkhorseXML += "		</Image>"
        'WorkhorseXML += "	</ShotGroup>"
        'WorkhorseXML += "</Page>"
        'WorkhorseXML += "</Ad>"
        'WorkhorseXML += "</JobInfo>"
        'WorkhorseXML += "</RequestDetail>"
        'WorkhorseXML += "</WorkhorseRequest>"

        ' BuildMyString.com generated code. Please enjoy your string responsibly.

        Dim WorkhorseXML As String
        WorkhorseXML = "<?xml version=""1.0"" encoding=""UTF-8""?><WorkhorseRequest xmlns:out=""http://schema.bonton.com/Schema/BT_WhMessage"" xmlns=""http://schema.bonton.com/Schema/BT_WhMessage"" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xsi:schemaLocation=""http://schema.bonton.com/Schema/BT_WhMessage http://workhorse.test.bonton.com/Schema/Bonton_WhMessage.xsd""> <RequestHeader> <SecurityInfo> <workhorseLogin>msg</workhorseLogin> <encryptedPassword>341A10747C4533634CA4F0588767AECDB39F2F5B5CD77C6640258CB8D47DF149</encryptedPassword> <mutualToken>BonTon</mutualToken> </SecurityInfo> <Operation>JobCreateOrUpdate</Operation> </RequestHeader> <RequestDetail> <JobInfo> <Ad> <AdNumber>21959</AdNumber> <AdDescription>11/19 - MASH JRS ROCAWEAR</AdDescription> <AdEnd>2009-11-22T00:00:00</AdEnd> <AdStart>2009-11-19T00:00:00</AdStart> <AdStatus>K</AdStatus> <AdVersion></AdVersion> <AssocFirst> </AssocFirst> <AssocId>0</AssocId> <AssocLast></AssocLast> <AssocPhone> </AssocPhone> <EventEnd>2009-11-28T00:00:00</EventEnd> <EventName>DMM Discretionary</EventName> <EventStart>2009-11-01T00:00:00</EventStart> <MediaDescription>Run of press </MediaDescription> <MediaType>BW + 3C</MediaType> <PhotoEnd>1900-01-01T00:00:00</PhotoEnd> <PhotoStart>1900-01-01T00:00:00</PhotoStart> <TurnInDate>2009-09-23T00:00:00</TurnInDate> <Page></Page> </Ad> </JobInfo> </RequestDetail> </WorkhorseRequest>"

        'WorkhorseXML = "<?xml version=""1.0"" encoding=""UTF-8""?><WorkhorseRequest xmlns:out=""http://workhorse.qa.bonton.com/Schema/BonTon_WhMessage"" xmlns=""http://workhorse.qa.bonton.com/Schema/BonTon_WhMessage"" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xsi:schemaLocation=""http://workhorse.qa.bonton.com/Schema/BonTon_WhMessage http://workhorse.qa.bonton.com/Schema/BonTon_WhMessage.xsd"">" &
        '"	<RequestHeader>" &
        '"		<SecurityInfo>" &
        '"			<workhorseLogin>msg</workhorseLogin>" &
        '"			<encryptedPassword>341A10747C4533634CA4F0588767AECDB39F2F5B5CD77C6640258CB8D47DF149</encryptedPassword>" &
        '"			<mutualToken>BonTon</mutualToken>" &
        '"		</SecurityInfo>" &
        '"		<Operation>JobCreateOrUpdate</Operation>" &
        '"	</RequestHeader>" &
        '"	<RequestDetail>" &
        '"		<JobInfo>" &
        '"			<Ad>" &
        '"				<AdNumber>2038</AdNumber>" &
        '"				<AdDescription>EC WK 16 SPRING 2013 T/I 4/17</AdDescription>" &
        '"				<AdEnd>0001-01-01T00:00:00</AdEnd>" &
        '"				<AdStart>2013-05-23T00:00:00</AdStart>" &
        '"				<AdStatus>A</AdStatus>" &
        '"				<AdVersion></AdVersion>" &
        '"				<AssocFirst>               </AssocFirst>" &
        '"				<AssocId>0</AssocId>" &
        '"				<AssocLast></AssocLast>" &
        '"				<AssocPhone>          </AssocPhone>" &
        '"				<EventEnd>1900-01-01T00:00:00</EventEnd>" &
        '"				<EventName>Web Promos</EventName>" &
        '"				<EventStart>1900-01-01T00:00:00</EventStart>" &
        '"				<MediaDescription>Web E-Commerce           </MediaDescription>" &
        '"				<MediaType>E-Commerce</MediaType>" &
        '"				<PhotoEnd>2013-05-03T00:00:00</PhotoEnd>" &
        '"				<PhotoStart>2013-04-25T00:00:00</PhotoStart>" &
        '"				<TurnInDate>2013-04-17T00:00:00</TurnInDate>" &
        '"				<Page>" &
        '"					<CoverPage>N</CoverPage>" &
        '"					<PageNumber>1</PageNumber>" &
        '"					<PageDescription>Women</PageDescription>" &
        '"					<WorkhorseJobId>2038</WorkhorseJobId>" &
        '"					<ActiveIndicator>A</ActiveIndicator>" &
        '"					<ShotGroup>" &
        '"						<ShotNumber>0</ShotNumber>" &
        '"						<Image>" &
        '"							<ImageNumber>696718</ImageNumber>" &
        '"							<Description>lbd p crcht frng vest 503a-tan</Description>" &
        '"							<ImageClass>On - Petite</ImageClass>" &
        '"							<ImageNotes>Fashion on figure</ImageNotes>" &
        '"							<ImageSource>N</ImageSource>" &
        '"							<ImageSuffixType></ImageSuffixType>" &
        '"							<MediaType>MORRIS/KARDELIS</MediaType>" &
        '"							<ActiveIndicator>A</ActiveIndicator>" &
        '"							<MerchForImage>" &
        '"								<MerchID>800522</MerchID>" &
        '"								<ActiveIndicator>A</ActiveIndicator>" &
        '"							</MerchForImage>" &
        '"						</Image>" &
        '"						<Image>" &
        '"							<ImageNumber>-1</ImageNumber>" &
        '"							<Description></Description>" &
        '"							<ImageClass></ImageClass>" &
        '"							<ImageNotes></ImageNotes>" &
        '"							<ImageSource></ImageSource>" &
        '"							<ImageSuffixType></ImageSuffixType>" &
        '"							<MediaType></MediaType>" &
        '"							<ActiveIndicator>A</ActiveIndicator>" &
        '"							<MerchForImage>" &
        '"								<MerchID>800522</MerchID>" &
        '"								<ActiveIndicator>A</ActiveIndicator>" &
        '"							</MerchForImage>" &
        '"							<MerchForImage>" &
        '"								<MerchID>800526</MerchID>" &
        '"								<ActiveIndicator>A</ActiveIndicator>" &
        '"							</MerchForImage>" &
        '"							<MerchForImage>" &
        '"								<MerchID>800510</MerchID>" &
        '"								<ActiveIndicator>A</ActiveIndicator>" &
        '"							</MerchForImage>" &
        '"							<MerchForImage>" &
        '"								<MerchID>800516</MerchID>" &
        '"								<ActiveIndicator>A</ActiveIndicator>" &
        '"							</MerchForImage>" &
        '"							<MerchForImage>" &
        '"								<MerchID>800149</MerchID>" &
        '"								<ActiveIndicator>A</ActiveIndicator>" &
        '"							</MerchForImage>" &
        '"						</Image>" &
        '"						<Image>" &
        '"							<ImageNumber>696720</ImageNumber>" &
        '"							<Description>lbd lace inst card 238a-ivry</Description>" &
        '"							<ImageClass>On - YC Missy (rs to 789456)</ImageClass>" &
        '"							<ImageNotes>Fashion on figure</ImageNotes>" &
        '"							<ImageSource>N</ImageSource>" &
        '"							<ImageSuffixType></ImageSuffixType>" &
        '"							<MediaType>MORRIS/KARDELIS</MediaType>" &
        '"							<ActiveIndicator>A</ActiveIndicator>" &
        '"							<MerchForImage>" &
        '"								<MerchID>800526</MerchID>" &
        '"								<ActiveIndicator>A</ActiveIndicator>" &
        '"							</MerchForImage>" &
        '"						</Image>" &
        '"					</ShotGroup>" &
        '"				</Page>" &
        '"			</Ad>" &
        '"			<Ad>" &
        '"				<AdNumber>17480</AdNumber>" &
        '"				<AdDescription>Q4 Merch ID Basket DNC NC</AdDescription>" &
        '"				<AdEnd>0001-01-01T00:00:00</AdEnd>" &
        '"				<AdStart>2015-11-01T00:00:00</AdStart>" &
        '"				<AdStatus>A</AdStatus>" &
        '"				<AdVersion></AdVersion>" &
        '"				<AssocFirst>               </AssocFirst>" &
        '"				<AssocId>0</AssocId>" &
        '"				<AssocLast></AssocLast>" &
        '"				<AssocPhone>          </AssocPhone>" &
        '"				<EventEnd>1900-01-01T00:00:00</EventEnd>" &
        '"				<EventName></EventName>" &
        '"				<EventStart>1900-01-01T00:00:00</EventStart>" &
        '"				<MediaDescription>Misc Other               </MediaDescription>" &
        '"				<MediaType>1 Color - Stores</MediaType>" &
        '"				<PhotoEnd>1900-01-01T00:00:00</PhotoEnd>" &
        '"				<PhotoStart>1900-01-01T00:00:00</PhotoStart>" &
        '"				<TurnInDate>1900-01-01T00:00:00</TurnInDate>" &
        '"				<Page>" &
        '"					<CoverPage></CoverPage>" &
        '"					<PageNumber>1</PageNumber>" &
        '"					<PageDescription></PageDescription>" &
        '"					<WorkhorseJobId>17480</WorkhorseJobId>" &
        '"					<ActiveIndicator>A</ActiveIndicator>" &
        '"					<ShotGroup>" &
        '"						<ShotNumber>-1</ShotNumber>" &
        '"						<Image>" &
        '"							<ImageNumber>-1</ImageNumber>" &
        '"							<Description></Description>" &
        '"							<ImageClass></ImageClass>" &
        '"							<ImageNotes></ImageNotes>" &
        '"							<ImageSource></ImageSource>" &
        '"							<ImageSuffixType></ImageSuffixType>" &
        '"							<MediaType></MediaType>" &
        '"							<ActiveIndicator>A</ActiveIndicator>" &
        '"							<MerchForImage>" &
        '"								<MerchID>800522</MerchID>" &
        '"								<ActiveIndicator>K</ActiveIndicator>" &
        '"							</MerchForImage>" &
        '"							<MerchForImage>" &
        '"								<MerchID>800526</MerchID>" &
        '"								<ActiveIndicator>K</ActiveIndicator>" &
        '"							</MerchForImage>" &
        '"							<MerchForImage>" &
        '"								<MerchID>800510</MerchID>" &
        '"								<ActiveIndicator>K</ActiveIndicator>" &
        '"							</MerchForImage>" &
        '"							<MerchForImage>" &
        '"								<MerchID>800516</MerchID>" &
        '"								<ActiveIndicator>K</ActiveIndicator>" &
        '"							</MerchForImage>" &
        '"						</Image>" &
        '"					</ShotGroup>" &
        '"				</Page>" &
        '"			</Ad>" &
        '"			<Ad>" &
        '"				<AdNumber>17476</AdNumber>" &
        '"				<AdDescription>Q1 Merch Id Basket DNC NC</AdDescription>" &
        '"				<AdEnd>0001-01-01T00:00:00</AdEnd>" &
        '"				<AdStart>2015-02-01T00:00:00</AdStart>" &
        '"				<AdStatus>A</AdStatus>" &
        '"				<AdVersion></AdVersion>" &
        '"				<AssocFirst>               </AssocFirst>" &
        '"				<AssocId>0</AssocId>" &
        '"				<AssocLast></AssocLast>" &
        '"				<AssocPhone>          </AssocPhone>" &
        '"				<EventEnd>1900-01-01T00:00:00</EventEnd>" &
        '"				<EventName></EventName>" &
        '"				<EventStart>1900-01-01T00:00:00</EventStart>" &
        '"				<MediaDescription>Misc Other               </MediaDescription>" &
        '"				<MediaType>1 Color - Stores</MediaType>" &
        '"				<PhotoEnd>1900-01-01T00:00:00</PhotoEnd>" &
        '"				<PhotoStart>1900-01-01T00:00:00</PhotoStart>" &
        '"				<TurnInDate>1900-01-01T00:00:00</TurnInDate>" &
        '"				<Page>" &
        '"					<CoverPage></CoverPage>" &
        '"					<PageNumber>3</PageNumber>" &
        '"					<PageDescription></PageDescription>" &
        '"					<WorkhorseJobId>17476</WorkhorseJobId>" &
        '"					<ActiveIndicator>A</ActiveIndicator>" &
        '"					<ShotGroup>" &
        '"						<ShotNumber>-1</ShotNumber>" &
        '"						<Image>" &
        '"							<ImageNumber>-1</ImageNumber>" &
        '"							<Description></Description>" &
        '"							<ImageClass></ImageClass>" &
        '"							<ImageNotes></ImageNotes>" &
        '"							<ImageSource></ImageSource>" &
        '"							<ImageSuffixType></ImageSuffixType>" &
        '"							<MediaType></MediaType>" &
        '"							<ActiveIndicator>A</ActiveIndicator>" &
        '"							<MerchForImage>" &
        '"								<MerchID>800149</MerchID>" &
        '"								<ActiveIndicator>K</ActiveIndicator>" &
        '"							</MerchForImage>" &
        '"						</Image>" &
        '"					</ShotGroup>" &
        '"				</Page>" &
        '"			</Ad>" &
        '"		</JobInfo>" &
        '"	</RequestDetail>" &
        '"</WorkhorseRequest>"



        Console.WriteLine(" Sending Request to Workhorse")
        WorkhorseHelper.SendRequest(WorkhorseXML)
        Console.WriteLine("Completed")
        Console.Read()
    End Sub

    Private Sub ValidateMerchandiseForTTU450(ByVal m As MerchandiseSample)
        Console.WriteLine("***************************************************************************")
        Console.WriteLine("************ Merchandise Debugger ************************")
        Console.WriteLine("***************************************************************************")
        If m.MerchID = "" Then
            Console.WriteLine("*** ERROR **** : : No merchID")
            Console.WriteLine("")
        Else
            Console.WriteLine("MerchId :: " & m.MerchID.ToString)
            Console.WriteLine("")
        End If

        If m.VendorStyleNumber = "" Then
            Console.WriteLine("*** ERROR **** : : No VendorStyle")
            Console.WriteLine("")
        Else
            Console.WriteLine("VendorStyle :: " & m.VendorStyleNumber.ToString)
            Console.WriteLine("")
        End If

        If m.PlaceholderAdNumber = 0 Then
            Console.WriteLine("*** ERROR **** : : No Ad Number")
            Console.WriteLine("")
        Else
            Console.WriteLine("Ad Number :: " & m.PlaceholderAdNumber.ToString)
            Console.WriteLine("")
        End If

        If m.PlaceholderPageNumber = 0 Then
            Console.WriteLine("*** ERROR **** : : No Page Number")
            Console.WriteLine("")
        Else
            Console.WriteLine("Page Number :: " & m.PlaceholderPageNumber.ToString)
            Console.WriteLine("")
        End If
        Console.WriteLine("Approval :: " & m.ApprovalFlag.ToString)
        Console.WriteLine("")
        If m.SampleStatus = "" Then
            Console.WriteLine("*** ERROR **** : : No Sample Status")
            Console.WriteLine("")
        Else
            Console.WriteLine("Sample Status :: " & m.SampleStatus.ToString)
            Console.WriteLine("")
        End If
        If m.SamplePrimaryLocation = "" Then
            Console.WriteLine("*** ERROR **** : : No Primary Location")
            Console.WriteLine("")
        Else
            Console.WriteLine("Primary Location :: " & m.SamplePrimaryLocation.ToString)
            Console.WriteLine("")
        End If
        If m.Requestor = "" Then
            Console.WriteLine("*** ERROR **** : : No Requestor")
            Console.WriteLine("")
        Else
            Console.WriteLine("Requestor :: " & m.Requestor.ToString)
            Console.WriteLine("")
        End If
        If m.SampleRequestedDate = DateTime.MinValue Then
            Console.WriteLine("*** ERROR **** : : No SampleRequestedDate")
            Console.WriteLine("")
        Else
            Console.WriteLine("SampleRequestedDate :: " & m.SampleRequestedDate.ToString)
            Console.WriteLine("")
        End If
        If m.User = "" Then
            Console.WriteLine("*** ERROR **** : : No User")
            Console.WriteLine("")
        Else
            Console.WriteLine("User :: " & m.User.ToString)
            Console.WriteLine("")
        End If
    End Sub
    Private Function DebugMerchandise(ByRef XML As String) As MerchandiseSample
        Dim m As MerchandiseSample = Helper.Deserialize(XML)
        Return m
    End Function


    Private Sub UpdateMerchandiseOnWHwithString(ByRef m As String)
        m = "<MerchandiseSample><ApprovalFlag>true</ApprovalFlag><ApprovalStatus>Approved</ApprovalStatus><BoxNumber>0</BoxNumber><BrandDesc>London Fog</BrandDesc><BrandId>641</BrandId><BuyerDesc>Boys 2-20</BuyerDesc><BuyerGroupEmail>BY53601@bonton.com</BuyerGroupEmail><BuyerId>360</BuyerId><ClassDesc>LONDON FOG OUTERWEAR</ClassDesc><ClassId>474</ClassId><CMGDesc>CHILDRENS</CMGDesc><CMGID>509</CMGID><ColorCode>0</ColorCode><ColorDesc>GREEN</ColorDesc><CRGDesc>CHILDRENS</CRGDesc><CRGID>300</CRGID><Disposition>6017</Disposition><DeptDesc>BOYSWEAR 2-7</DeptDesc><DeptID>244</DeptID><ExpediteReturn>false</ExpediteReturn><ISN>244109697</ISN><ISNChanged>true</ISNChanged><ISNDescription>+ACT CB PUFFER JKT *LF</ISNDescription><IsWebEligible>false</IsWebEligible><LabelDesc>London Fog</LabelDesc><LabelID>641</LabelID><MerchID>854534</MerchID><PlaceholderAdNumber>17477</PlaceholderAdNumber><PlaceholderPageNumber>3</PlaceholderPageNumber><Quantity>1</Quantity><Requestor>SR/991003</Requestor><SampleDueInhouseDate>2015-06-22T00:00:00</SampleDueInhouseDate><SamplePrimaryLocation>CMR - Centralized Merch Room</SamplePrimaryLocation><SampleRequestType>SAMPLE</SampleRequestType><SampleSource>6040</SampleSource><SampleStatus>Received</SampleStatus><SellingLocation>false</SellingLocation><SequenceNumber>0</SequenceNumber><ShelfNumber>0</ShelfNumber><SizeCode>491</SizeCode><SizeDesc>5-6</SizeDesc><TurninMerchandiseID>0</TurninMerchandiseID><UPC>700140176336</UPC><User>jdimond</User><VendorEmail>fran.metzler@amerexgroup.com</VendorEmail><VendorId>4340</VendorId><VendorInstructions>Web Sample, send any size available</VendorInstructions><VendorName>AMEREX</VendorName><VendorPID>L215E17-BT</VendorPID><VendorStyleNumber>L215E17-BT</VendorStyleNumber><SnapshotImage><PrimaryView><ActualURL>https://s3-us-west-2.amazonaws.com/bonton-assets/558b33e6a167e/large-854534.JPG</ActualURL><MediumURL>https://s3-us-west-2.amazonaws.com/bonton-assets/558b33e6a167e/medium-854534.JPG</MediumURL><ThumbnailURL>https://s3-us-west-2.amazonaws.com/bonton-assets/558b33e6a167e/thumbnail-854534.JPG</ThumbnailURL></PrimaryView></SnapshotImage></MerchandiseSample>"
        Dim RequestMerchandiseXML As String = m
        Dim WHRequest As String = WorkhorseHelper.CreateRequest(WorkhorseHelper.Operation.MerchUpdate, m)
        Dim WHresponse As String = WorkhorseHelper.SendRequest(WHRequest)
        Dim o As New MerchandiseSample
        WorkhorseHelper.ProcessReponse(WHresponse, o)
    End Sub

    Private Sub UpdateMerchandiseOnWH(ByRef m As MerchandiseSample)
        Dim RequestMerchandiseXML As String = Helper.Serialize(m)
        Dim WHRequest As String = WorkhorseHelper.CreateRequest(m, WorkhorseHelper.Operation.MerchUpdate)
        Dim WHresponse As String = WorkhorseHelper.SendRequest(WHRequest)
        WorkhorseHelper.ProcessReponse(WHresponse, m)

        ReportMerchandise(m)
    End Sub


    Private Sub GetMerchandiseFromWH(ByRef m As MerchandiseSample)
        Dim RequestMerchandiseXML As String = Helper.Serialize(m)
        Dim WHRequest As String = WorkhorseHelper.CreateRequest(m, WorkhorseHelper.Operation.MerchGet)
        Dim WHresponse As String = WorkhorseHelper.SendRequest(WHRequest)
        WorkhorseHelper.ProcessReponse(WHresponse, m)

        ReportMerchandise(m)
    End Sub
   
    Private Sub SendNewMerchandiseToWorkhorse(ByVal ISN As Decimal)
        Dim mList As New List(Of MerchandiseSample)
        mList = GetNewMerchandiseList(ISN)

        For Each obj As MerchandiseSample In mList
            CreateNewMerchandise(obj)
        Next
    End Sub

    Private Sub CreateNewMerchandise(ByVal m As MerchandiseSample)
        'Dim merchId As Integer = 800013
        'Dim m As New MerchandiseSample
        'm = GenerateMerchandise(merchId, WorkhorseHelper.Operation.MerchCreate)
        Dim RequestMerchandiseXML As String = Helper.Serialize(m)
        Dim WHRequest As String = WorkhorseHelper.CreateRequest(m, WorkhorseHelper.Operation.MerchCreate)
        Dim WHresponse As String = WorkhorseHelper.SendRequest(WHRequest)
        WorkhorseHelper.ProcessReponse(WHresponse, m)
        Dim ResponseMerchandiseXML As String = Helper.Serialize(m)
        ReportMerchandise(m)
        InsertMerchandise(m)
        UpdateAdministrationDatabase(m)

        CompareObjects(RequestMerchandiseXML, ResponseMerchandiseXML, m.MerchID.ToString)
    End Sub

    Private Sub ReportMerchandise(ByVal m As MerchandiseSample)
        Console.WriteLine("Merch ID (Barcode) : " & m.MerchID)
        '[Choose Sample Status] Disposed In Production Returned Requested Received In Transit Moving Returned Shipped Donated Retreived R 
        Console.WriteLine("Sample Status  : " & m.SampleStatus)
        '  [Choose Active Indicator] A K   
        Console.WriteLine("Active Indicator : " & m.SampleStatus)
        Console.WriteLine("Checked In Date : " & m.CheckInDate)
        Console.WriteLine("Changed Date : " & m.LastUsedDate)
        '  [Choose Primary Location] Bon-Ton HQ Bon-Ton Layton Milwaukee-CMR   
        Console.WriteLine("Primary Location : " & m.SamplePrimaryLocation)
        Console.WriteLine("Current Location : " & m.SampleSecondaryLocation)
        'ReceivedatCorporate Off Site Merch Room MRCH RM - Ready for Styling Styling Room STYL RM - Ready for Photography Studio 1 Studio 2 Studio 3 Studio 4 Studio 5 Studio 6 Studio 7 Studio 8 Studio 9 Studio 10 Studio 11 Studio 12 Studio 13 Studio 14 OFF SITE - Quad OFF SITE - Broadcast OFF SITE - Location Special Projects Damaged Out Merch Fashion Prop Hold -------------------------------------------------- NotReceived ReturnedtoCorporate RoutedtoReturnRoom ReturnedtoBuyer ReturnedtoMerchRoom Received at Corporate   
        Console.WriteLine("Location : " & m.SampleSecondaryLocation)
        Console.WriteLine("Box #  : " & m.BoxNumber)
        Console.WriteLine("Shelf #: " & m.ShelfNumber)
        '   Active Inactive   
        Console.WriteLine("Active Status : " & m.ApprovalStatus)
        '[Choose] Not Render Set Feature Swatch Site Feature Render Swatch   
        Console.WriteLine("Render Set Grouping  : ????????")
        Console.WriteLine("Feature Image Id : ??????????")

        Console.WriteLine("Item Description : " & m.ISNDescription)

        Console.WriteLine("Dept Name : " & m.DeptDesc)
        Console.WriteLine("Dept #   : " & m.DeptID)
        Console.WriteLine("Color : " & m.ColorDesc & "(" & m.ColorCode & ")")

        Console.WriteLine("Size : " & m.SizeDesc & "(" & m.SizeCode & ")")

        Console.WriteLine("Style # : " & m.VendorStyleNumber)

        Console.WriteLine("Class : " & m.ClassDesc & "(" & m.ClassId & ")")
        Console.WriteLine("Vendor Resource: ?????????????")
        Console.WriteLine("UPC : " & m.UPC)
        Console.WriteLine("Turn In Date :   -------------   ")
        Console.WriteLine("Extension Date : " & m.ExtensionDate.ToString)
        Console.WriteLine("Qty : " & m.Quantity.ToString)

        Console.WriteLine(" --------------       Identifiers     ------------------")

        Console.WriteLine("ISN : " & m.ISN.ToString)

        Console.WriteLine("        ISN Changed : " & m.ISNChanged)


        Console.WriteLine("CMG ID : " & m.CMGID.ToString)
        Console.WriteLine("CMG Desc : " & m.CMGDesc)
        Console.WriteLine("CRG ID : " & m.CRGID.ToString)
        Console.WriteLine("CRG Desc : " & m.CRGDesc)
        Console.WriteLine("Class Description : " & m.ClassDesc)

        Console.WriteLine(" --------------       Attributes     ------------------")

        Console.WriteLine("Sample Approved : " & m.ApprovalFlag)
        Console.WriteLine("Sample Approval Type : " & m.ApprovalStatus)
        Console.WriteLine("Color Code  : " & m.ColorCode)
        Console.WriteLine("Size Code  : " & m.SizeCode)
        Console.WriteLine("Brand  : " & m.BrandDesc)
        Console.WriteLine("Brand ID  : " & m.BrandId)
        Console.WriteLine("Label : " & m.LabelDesc)
        Console.WriteLine("Label ID  : " & m.LabelID)

        Console.WriteLine("Web Eligible  : " & m.SellingLocation)

        Console.WriteLine("Web Only : " & m.IsWebEligible)


        Console.WriteLine("Sequence  : ????????????")

        Console.WriteLine("Sample Requestor : " & m.Requestor)

        Console.WriteLine("Sample Requested Date  : " & m.SampleRequestedDate)
        Console.WriteLine("Sample Requestor Name : " & m.Requestor)
        Console.WriteLine("Sample Requestor Email  : " & m.BuyerGroupEmail)
        Console.WriteLine("Request Type  : " & m.SampleRequestType)
        Console.WriteLine("Inhouse Date  : " & m.SampleDueInhouseDate)
        Console.WriteLine("Sample Source : " & m.SampleSource)

        Console.WriteLine("Turn In Merchandise ID : " & m.TurninMerchandiseID)

        Console.WriteLine("Turn In User  : " & m.User)

        Console.WriteLine("Sample Disposition  : " & m.Disposition)

        Console.WriteLine(" --------------       Vendor Information     ------------------")

        Console.WriteLine(" Vendor ID   : " & m.VendorId)
        Console.WriteLine("Vendor Name   : " & m.VendorName)
        Console.WriteLine("Vendor PID  : " & m.VendorPID)
        Console.WriteLine("Vendor Email   : " & m.VendorEmail)
        Console.WriteLine("Vendor Phone   : " & m.VendorPhone)
        Console.WriteLine("Vendor Instructions   : " & m.VendorInstructions)


        Console.WriteLine("Expedite Return   : " & m.ExpediteReturn)
        Console.WriteLine("Extension Date   : " & m.ExtensionDate)

        Console.WriteLine("Return Date    : " & m.ReturnDate)
        Console.WriteLine("Vendor Return Address 1    : " & m.ReturnAddress1)
        Console.WriteLine("Vendor Return Address 2    : " & m.ReturnAddress2)
        Console.WriteLine("Vendor Return Address City    : " & m.ReturnCity)
        Console.WriteLine("Vendor Return Address State    : " & m.ReturnState)
        Console.WriteLine("Vendor Return Address Zip    : " & m.ReturnZip)
        Console.WriteLine("Vendor Return Phone Number    : " & m.ReturnPhone)


        Console.WriteLine(" --------------       Buyer Information     ------------------")

        Console.WriteLine("Buyer ID : " & m.BuyerId)
        Console.WriteLine("Buyer Desc : " & m.BuyerDesc)
        Console.WriteLine("Buying Office Email ID : " & m.BuyerGroupEmail)

        Console.WriteLine(" --------------       Notes     ------------------")

        Console.WriteLine("CMR Notes : " & m.CMRNotes)
        Console.WriteLine("Styleing Notes   : " & m.SampleCareNotes)


        Console.WriteLine(" --------------       URLs     ------------------")

        Console.WriteLine("File Name : ??????????????")
        Console.WriteLine("Primary Thumbnail URL : " & m.SnapshotImage.PrimaryView.ThumbnailURL)
        Console.WriteLine("Primary Medium URL  : " & m.SnapshotImage.PrimaryView.MediumURL)
        Console.WriteLine("Primary Actual URL  : " & m.SnapshotImage.PrimaryView.ActualURL)
        Console.WriteLine("Secondary Thumbnail URL  : " & m.SnapshotImage.SecondaryView.ThumbnailURL)
        Console.WriteLine("Secondary Medium URL   : " & m.SnapshotImage.SecondaryView.MediumURL)
        Console.WriteLine("Secondary Actual URL   : " & m.SnapshotImage.SecondaryView.ActualURL)

    End Sub

    Private Sub Workhorse_NewMerch_Stress_Test(ByVal ISNList As List(Of Integer))
        For Each ISN As Integer In ISNList
            Dim mList As New List(Of MerchandiseSample)
            mList = GetNewMerchandiseList(ISN)
            If mList.Count > 0 Then
                For Each m As MerchandiseSample In mList
                    Dim SerializedXML As String = SerializeMerchandiseObject(m)
                    WriteToFile(SerializedXML, m.MerchID, "ToWH")
                Next
            End If

            Dim WHRequest As String = WorkhorseHelper.CreateRequest(GenerateMerchandise(0, WorkhorseHelper.Operation.MerchCreate), WorkhorseHelper.Operation.MerchCreate)
            WorkhorseHelper.SendRequest(WHRequest)
        Next
    End Sub

    Private Sub MessageBroker_Stress_Test(ByVal ISNList As List(Of Integer))

    End Sub

    Private Function GenerateMerchandise(ByVal merchId As Integer, ByVal Op As WorkhorseHelper.Operation) As MerchandiseSample
        Dim m As New MerchandiseSample
        m = GetMerchandise(merchId)
        If Not IsNothing(m) Then
            If Op = WorkhorseHelper.Operation.MerchCreate Then
                m.MerchID = 0
            ElseIf Op = WorkhorseHelper.Operation.MerchUpdate Then
                m.MerchID = merchId
                m.SampleStatus = "A"
            End If

            Return m
        Else
            Return Nothing
        End If
    End Function

    Private Sub GenerateMerchandiseXMLToFolder(ByVal merchId As Integer)
        Dim m As New MerchandiseSample
        m = GetMerchandise(merchId)
        Dim SerializedXML As String = SerializeMerchandiseObject(m)
        WriteToFile(SerializedXML, m.MerchID, "FromWH")
        Console.WriteLine(SerializedXML)
        'LoadMerchandiseFromCSV(True, True)
        Console.Read()
    End Sub

    Private Sub GenerateNewMerchandiseXMLToQueue(ByVal ISN As Decimal)
        Dim mList As New List(Of MerchandiseSample)
        mList = GetNewMerchandiseList(ISN)
        Console.WriteLine("Total Messages written : " & mList.Count.ToString)
        If mList.Count > 0 Then
            For Each m As MerchandiseSample In mList
                Dim SerializedXML As String = SerializeMerchandiseObject(m)
                WriteToQueue(SerializedXML, "ToWH")
            Next
        End If
    End Sub

    Private Sub GenerateNewMerchandiseXMLToFolder(ByVal ISN As Decimal)
        Dim mList As New List(Of MerchandiseSample)
        mList = GetNewMerchandiseList(ISN)
        If mList.Count > 0 Then
            For Each m As MerchandiseSample In mList
                Dim SerializedXML As String = SerializeMerchandiseObject(m)
                WriteToFile(SerializedXML, m.MerchID, "ToWH")
            Next
        End If
    End Sub
    ''' <summary>
    '''  Use the query in this function to generate the Excel Input for the
    ''' </summary>
    ''' <remarks></remarks>
    ''' 

    Private Sub PopulateExcel()
        '       select 0 AS SAMPLE_MERCH_ID , ISN.INTERNAL_STYLE_NUM, ISN.VENDOR_STYLE_NUM, ISN.ISN_LONG_DESC, SKU.CLR_CDE, 
        'CLR.CLR_DESC  AS CLR_LONG_DESC,
        ' 5006 AS AD_NUM, SKU.SIZE_ID, CURRENT TIMESTAMP AS SAMPLE_DUE_DTE
        ' , 'N' AS SAMPLE_APVL_FLG,
        '            'Sample' AS SAMPLE_REQUEST_TYP,
        ' size_1_desc || Size_SEP || Size_2_Desc AS SAMPLE_SIZE_DESC,
        '            'P' AS SAMPLE_STATUS_DESC,
        '            'Investigation Needed' AS SAMPLE_APVL_TYP,
        '            'Milwaukee-CMR' AS SMPL_PRIM_LOC_NME,
        '            'Generate' AS SMPL_REQ_CRTE_NME,
        ' CURRENT TIMESTAMP AS SMPL_REQ_CRTE_TS,
        '            'Abarde' AS LAST_MOD_ID,
        ' CURRENT TIMESTAMP AS LAST_MOD_TS,
        ' ISN.DEPT_ID,
        ' SKU.UPC_NUM AS UPC_NUM,
        '            'http://assets.libsyn.com/images/imake/yourpodcastartwork.jpg'	AS PRIM_ACTL_URL_TXT,
        '            'http://www.upgradedimages.com/images/pixels/600x600.jpg' AS PRIM_MED_URL_TXT ,
        '            'http://www.upgradedimages.com/images/pixels/300x300.jpg'	AS PRIM_THB_URL_TXT,
        '            'http://assets.libsyn.com/images/imake/yourpodcastartwork.jpg'	AS SEC_ACTL_URL_TXT,         
        '            'http://www.upgradedimages.com/images/pixels/600x600.jpg' AS SEC_MED_URL_TXT,	
        '            'http://www.upgradedimages.com/images/pixels/300x300.jpg' AS SEC_THB_URL_TXT
        '   from TSS200SKU SKU
        'INNER JOIN MRSSI.TSS100ISN ISN ON ISN.INTERNAL_STYLE_NUM = SKU.INTERNAL_STYLE_NUM 
        'INNER JOIN MRSSI.TSS101ISN_COLOR CLR ON SKU.INTERNAL_STYLE_NUM = CLR.INTERNAL_STYLE_NUM AND  SKU.CLR_CDE = CLR.CLR_CDE
        'INNER JOIN MRSSI.TSS102ISN_SIZE SIZ ON SKU.INTERNAL_STYLE_NUM = SIZ.INTERNAL_STYLE_NUM AND  SKU.SIZE_ID = SIZ.SIZE_ID
        'INNER JOIN MRSSI.TCD333SIZE SIZD ON SIZ.SIZE_ID = SIZD.SIZE_ID 
        'where ISN.internal_style_num = 61100126;
    End Sub

    Private Sub LoadMerchandiseFromCSV(ByVal GenerateXMLOnly As Boolean, ByVal GenerateMerchId As Boolean)
        Using parser As New TextFieldParser(CSVPullfolder)
            parser.CommentTokens = New String() {"#"}
            parser.SetDelimiters(New String() {","})
            parser.HasFieldsEnclosedInQuotes = True

            parser.ReadLine()
            Dim transactionType As String
            While Not parser.EndOfData
                Dim fields As String() = parser.ReadFields()
                Dim m As New MerchandiseSample
                If GenerateMerchId Then
                    m.MerchID = fields(0)
                    transactionType = "FromWH"
                Else
                    m.MerchID = 0
                    transactionType = "ToWH"
                End If

                m.ISN = fields(1)
                m.VendorStyleNumber = fields(2)
                m.ISNDescription = fields(3)
                m.ColorCode = fields(4)
                m.ColorDesc = fields(5)
                m.PlaceholderAdNumber = fields(6)
                m.SizeCode = fields(7)
                m.SampleDueInhouseDate = fields(8)
                m.ApprovalStatus = fields(9)
                m.SampleRequestType = fields(10)
                m.SizeDesc = fields(11)
                m.SampleStatus = fields(12)
                'm.SampleApprovalStatus = fields(13)
                m.SamplePrimaryLocation = fields(14)
                Dim image As New TurnInProcessAutomation.BusinessEntities.SnapshotImage
                Dim mpURL As New TurnInProcessAutomation.BusinessEntities.URL
                mpURL.ActualURL = fields(22)
                mpURL.MediumURL = fields(23)
                mpURL.ThumbnailURL = fields(24)
                image.PrimaryView = mpURL
                Dim msURL As New TurnInProcessAutomation.BusinessEntities.URL
                msURL.ActualURL = fields(25)
                msURL.MediumURL = fields(26)
                msURL.ThumbnailURL = fields(27)
                image.SecondaryView = msURL
                m.SnapshotImage = image
                m.DeptID = fields(20)
                m.DeptDesc = "xxxx"
                m.PlaceholderPageNumber = 1
                m.CMGID = 1
                m.CMGDesc = "Test CMG"
                m.ApprovalStatus = "T"
                m.ApprovalFlag = "Y"
                m.User = "abarde"
                m.UPC = fields(21)

                If GenerateXMLOnly Then
                    Console.WriteLine()
                    Dim SerializedXML As String = SerializeMerchandiseObject(m)
                    WriteToFile(SerializedXML, m.MerchID, transactionType)
                    Console.WriteLine(SerializedXML)
                Else
                    InsertMerchandise(m)
                    UpdateAdministrationDatabase(m)
                End If

            End While
        End Using
        Console.WriteLine("Completed Execution")
    End Sub

    Private Function SerializeRequestObject(ByVal request As RequestDetail) As String
        Return Helper.Serialize(request)
    End Function

    Private Function SerializeMerchandiseObject(ByVal merchandise As MerchandiseSample) As String
        Return Helper.Serialize(merchandise)
    End Function

    Private Sub WriteToFile(ByVal XMLString As String, ByVal prefixMerchId As String, ByVal TransactionType As String)
        Dim location As String = ""
        Select Case TransactionType
            Case "ToWH"
                location = XMLToWHPushfolder
            Case "FromWH"
                location = XMLFromWHPushfolder
        End Select
        Using outfile As StreamWriter = New StreamWriter(location + "\MerchandiseSchema" & prefixMerchId & "_" & R.Next(1, 10000).ToString & ".xml", True)
            outfile.Write(XMLString)
        End Using
    End Sub

    Private Sub WriteToQueue(ByVal XMLString As String, ByVal TransactionType As String)
        Dim x As New MQ
        x.connectToMQ("wmq.test.bonton.com", 5075, "APPL.TU.WH.CHANNEL")
        Console.WriteLine(x.PutToMQ("APPL.SS.MERCH.CREATE.WEB.INBOUND.QUEUE", XMLString, "MessageBrokerTEst", DateTime.Now.ToString))
        x.disconnectFromMQ()
    End Sub

    Private Function GetNewMerchandiseList(ByVal ISN As Integer) As List(Of MerchandiseSample)
        Dim cmdparms As DB2Parameter() = New DB2Parameter() {New DB2Parameter("@TEST_ISN", DB2Type.Decimal)}
        cmdparms(0).Value = ISN

        Dim cmd As New DB2Command()
        cmd.CommandTimeout = 0
        'Dim conn As New DB2Connection("Server=gateway.db.prod.bonton.com:6100;Database=PROD;UID=CSTUTRNP;PWD=kct#r4by;Persist Security Info=True")
        Dim conn As New DB2Connection("Server=gateway.db.test.bonton.com:6100;Database=TEST;UID=TCABARD;PWD=bont1988;Persist Security Info=True")

        If conn.State <> ConnectionState.Open Then
            conn.Open()
        End If
        cmd.Connection = conn

        cmd.CommandType = CommandType.StoredProcedure
        cmd.CommandText = "TC.TU1141SP"

        If cmdparms IsNot Nothing Then
            For Each parm As DB2Parameter In cmdparms
                cmd.Parameters.Add(parm)
            Next
        End If
        Dim merchObject As New List(Of MerchandiseSample)
        Try
            Dim rdr As DB2DataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection)
            cmd.Parameters.Clear()
            merchObject = MerchandiseFactory.ConstructList(rdr)
            rdr.Close()
        Catch ex As Exception
            Console.WriteLine(ex.Message)
            Console.Read()
        End Try
        Return merchObject
    End Function

    Private Function GetMerchandise(ByVal MerchId As Integer) As MerchandiseSample
        Dim cmdparms As DB2Parameter() = New DB2Parameter() {New DB2Parameter("@MerchId", DB2Type.Integer)}
        cmdparms(0).Value = MerchId

        Dim cmd As New DB2Command()
        cmd.CommandTimeout = 0
        'Dim conn As New DB2Connection("Server=gateway.db.prod.bonton.com:6100;Database=PROD;UID=CSTUTRNP;PWD=kct#r4by;Persist Security Info=True")
        Dim conn As New DB2Connection("Server=gateway.db.test.bonton.com:6100;Database=TEST;UID=TCABARD;PWD=bont1234;Persist Security Info=True")

        If conn.State <> ConnectionState.Open Then
            conn.Open()
        End If
        cmd.Connection = conn

        cmd.CommandType = CommandType.StoredProcedure
        cmd.CommandText = "TC.TU1140SP"

        If cmdparms IsNot Nothing Then
            For Each parm As DB2Parameter In cmdparms
                cmd.Parameters.Add(parm)
            Next
        End If

        Try

            Dim rdr As DB2DataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection)
            cmd.Parameters.Clear()
            If rdr.HasRows Then
                Dim merchObject As New MerchandiseSample
                merchObject = MerchandiseFactory.Construct(rdr)
                rdr.Close()
                Return merchObject
            End If


        Catch ex As Exception
            Console.WriteLine(ex.Message)
            Console.Read()
        End Try


    End Function



    Private Sub UpdateAdministrationDatabase(ByVal m As MerchandiseSample)
        Dim parms As SqlParameter() = New SqlParameter() {New SqlParameter("@in_admin_merch_num", SqlDbType.Int, 0), _
                                                          New SqlParameter("@sku_upc", SqlDbType.Decimal), _
                                                          New SqlParameter("@merch_desc", SqlDbType.Char, 60), _
                                                          New SqlParameter("@vend_style_nbr", SqlDbType.Char, 20), _
                                                          New SqlParameter("@size", SqlDbType.Char, 10), _
                                                          New SqlParameter("@color", SqlDbType.Char, 15), _
                                                          New SqlParameter("@vendor_color_code", SqlDbType.SmallInt), _
                                                          New SqlParameter("@dept_nbr", SqlDbType.SmallInt), _
                                                          New SqlParameter("@isn", SqlDbType.Decimal), _
                                                          New SqlParameter("@ad_nbr", SqlDbType.Int), _
                                                          New SqlParameter("@page_nbr", SqlDbType.SmallInt), _
                                                         New SqlParameter("@status", SqlDbType.Char, 30)}
        Dim strFriendlyProdDesc As String = String.Empty

        ' Set up the parameters 
        parms(0).Value = m.MerchID
        parms(1).Value = m.UPC
        parms(2).Value = m.ISNDescription
        parms(3).Value = m.VendorStyleNumber.Trim
        parms(4).Value = m.SizeCode

        If m.ColorDesc.Length > 15 Then
            parms(5).Value = m.ColorDesc.Substring(0, 15).Trim
        Else
            parms(5).Value = m.ColorDesc.Trim
        End If

        parms(6).Value = m.ColorCode
        parms(7).Value = CShort(m.DeptID)
        parms(8).Value = m.ISN
        parms(9).Value = CInt(m.PlaceholderAdNumber)
        parms(10).Value = m.PlaceholderPageNumber
        parms(11).Value = IIf(m.SampleStatus = "Received", "A", "K")

        Try
            Dim cmd As New SqlCommand()
            Try
                Using conn As New SqlConnection("Server=M055-SQL-1T;Initial Catalog=DBADVTEST;User ID=INFORMIX;Password=INFORMIX;Persist Security Info=True")
                    If conn.State <> ConnectionState.Open Then
                        conn.Open()
                    End If
                    cmd.Connection = conn
                    cmd.CommandText = "tu_admin_data_insert"

                    cmd.CommandType = CommandType.StoredProcedure
                    If parms IsNot Nothing Then
                        For Each parm As SqlParameter In parms
                            cmd.Parameters.Add(parm)
                        Next
                    End If

                    Dim val As Object = cmd.ExecuteNonQuery
                    cmd.Parameters.Clear()
                    cmd.Dispose()
                End Using
            Catch ex As Exception
                Throw
            Finally
                cmd = Nothing
            End Try

        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Private Sub InsertMerchandise(ByVal m As MerchandiseSample)

        Dim cmdparms As DB2Parameter() = New DB2Parameter() {New DB2Parameter("@MerchId", DB2Type.Integer), _
                                                          New DB2Parameter("@ISN", DB2Type.Decimal), _
                                                          New DB2Parameter("@VendorStyleNumber", DB2Type.VarChar), _
                                                          New DB2Parameter("@ISNDescription", DB2Type.VarChar), _
                                                          New DB2Parameter("@ColorCode", DB2Type.Integer), _
                                                          New DB2Parameter("@ColorDesc", DB2Type.VarChar), _
                                                          New DB2Parameter("@PlaceholderAdNumber", DB2Type.Integer), _
                                                          New DB2Parameter("@SizeCode", DB2Type.Integer), _
                                                          New DB2Parameter("@SampleDueInhouseDate", DB2Type.VarChar), _
                                                          New DB2Parameter("@ApprovalFlag", DB2Type.VarChar), _
                                                          New DB2Parameter("@SampleRequestType", DB2Type.VarChar), _
                                                          New DB2Parameter("@SizeDesc", DB2Type.VarChar), _
                                                          New DB2Parameter("@SampleStatus", DB2Type.VarChar),
                                                          New DB2Parameter("@ApprovalStatus", DB2Type.VarChar), _
                                                          New DB2Parameter("@SamplePrimaryLocation", DB2Type.VarChar), _
                                                          New DB2Parameter("@PrimaryActualURL", DB2Type.VarChar), _
                                                          New DB2Parameter("@Requestor", DB2Type.VarChar), _
                                                          New DB2Parameter("@SampleRequestedDate", DB2Type.VarChar), _
                                                          New DB2Parameter("@User", DB2Type.VarChar), _
                                                          New DB2Parameter("@DeptId", DB2Type.Integer), _
                                                          New DB2Parameter("@UPC", DB2Type.Decimal), _
                                                          New DB2Parameter("@PrimaryMediumURL", DB2Type.VarChar), _
                                                          New DB2Parameter("@PrimaryThumbnailURL", DB2Type.VarChar), _
                                                          New DB2Parameter("@SecondaryActualURL", DB2Type.VarChar), _
                                                          New DB2Parameter("@SecondaryMediumURL", DB2Type.VarChar), _
                                                          New DB2Parameter("@SecondaryThumbnailURL", DB2Type.VarChar)}

        cmdparms(0).Value = m.MerchID
        cmdparms(1).Value = m.ISN
        cmdparms(2).Value = m.VendorStyleNumber
        cmdparms(3).Value = m.ISNDescription
        cmdparms(4).Value = m.ColorCode
        cmdparms(5).Value = m.ColorDesc
        cmdparms(6).Value = m.PlaceholderAdNumber
        cmdparms(7).Value = m.SizeCode
        cmdparms(8).Value = m.SampleDueInhouseDate
        cmdparms(9).Value = If(m.ApprovalFlag, "Y", "N")
        cmdparms(10).Value = m.SampleRequestType
        cmdparms(11).Value = m.SizeDesc
        cmdparms(12).Value = m.SampleStatus
        cmdparms(13).Value = m.ApprovalStatus
        cmdparms(14).Value = m.SamplePrimaryLocation
        cmdparms(15).Value = m.SnapshotImage.PrimaryView.ActualURL
        cmdparms(16).Value = m.User
        cmdparms(17).Value = DateTime.Now
        cmdparms(18).Value = m.User
        cmdparms(19).Value = m.DeptID
        cmdparms(20).Value = m.UPC
        cmdparms(21).Value = m.SnapshotImage.PrimaryView.MediumURL
        cmdparms(22).Value = m.SnapshotImage.PrimaryView.ThumbnailURL
        cmdparms(23).Value = m.SnapshotImage.SecondaryView.ActualURL
        cmdparms(24).Value = m.SnapshotImage.SecondaryView.MediumURL
        cmdparms(25).Value = m.SnapshotImage.SecondaryView.ThumbnailURL

        Dim cmd As New DB2Command()
        cmd.CommandTimeout = 0
        'Dim conn As New DB2Connection("Server=gateway.db.prod.bonton.com:6100;Database=PROD;UID=CSTUTRNP;PWD=kct#r4by;Persist Security Info=True")
        Dim conn As New DB2Connection("Server=gateway.db.test.bonton.com:6100;Database=TEST;UID=TCABARD;PWD=bont1234;Persist Security Info=True")

        If conn.State <> ConnectionState.Open Then
            conn.Open()
        End If
        cmd.Connection = conn

        cmd.CommandType = CommandType.StoredProcedure
        cmd.CommandText = "TC.TU1119SP"

        If cmdparms IsNot Nothing Then
            For Each parm As DB2Parameter In cmdparms
                cmd.Parameters.Add(parm)
            Next
        End If

        Try
            cmd.ExecuteNonQuery()
        Catch ex As Exception
            Console.WriteLine(ex.Message)
            Console.Read()
        End Try

    End Sub

    Dim counter As Integer = 0
    Public Sub CompareObjects(ByVal Request As String, ByVal Response As String, ByVal MerchId As String)

        Using outfile As StreamWriter = New StreamWriter("C:\temp\CompareCMR\Request" & MerchId & ".xml", True)
            outfile.Write(Request)
        End Using

        Using outfile As StreamWriter = New StreamWriter("C:\temp\CompareCMR\Response" & MerchId & ".xml", True)
            outfile.Write(Response)
        End Using

        counter += 1
    End Sub

    Private Sub DeleteCompareFIles()
        DeleteFromDirectory(ComparePath)
    End Sub

    Private Sub DeleteFromDirectory(ByVal Path As String)
        For Each s In System.IO.Directory.GetFiles(Path)
            System.IO.File.Delete(s)
        Next s
    End Sub
End Module


