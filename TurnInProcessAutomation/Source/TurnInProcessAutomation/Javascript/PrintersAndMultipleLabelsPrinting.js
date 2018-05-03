//----------------------------------------------------------------------------
//
//  $Id: PreviewAndPrintLabel.js 10794 2010-01-15 20:31:49Z vbuzuev $ 
//
// Project -------------------------------------------------------------------
//
//  DYMO Label Framework
//
// Content -------------------------------------------------------------------
//
//  DYMO Label Framework JavaScript Library Samples: Printers Information
//
//----------------------------------------------------------------------------
//
//  Copyright (c), 2010, Sanford, L.P. All Rights Reserved.
//
//----------------------------------------------------------------------------
(function () {

    // any label layout is a simple layout with one Text object
    var dieCutLabelLayout = '<?xml version="1.0" encoding="utf-8"?>\
<DieCutLabel Version="8.0" Units="twips">\
	<PaperOrientation>Landscape</PaperOrientation>\
	<Id>Address</Id>\
	<PaperName>30252 Address</PaperName>\
	<DrawCommands>\
		<RoundRectangle X="0" Y="0" Width="1581" Height="5040" Rx="270" Ry="270" />\
	</DrawCommands>\
	<ObjectInfo>\
		<TextObject>\
			<Name>TEXT</Name>\
			<ForeColor Alpha="255" Red="0" Green="0" Blue="0" />\
			<BackColor Alpha="0" Red="255" Green="255" Blue="255" />\
			<LinkedObjectName></LinkedObjectName>\
			<Rotation>Rotation0</Rotation>\
			<IsMirrored>False</IsMirrored>\
			<IsVariable>False</IsVariable>\
			<HorizontalAlignment>Left</HorizontalAlignment>\
			<VerticalAlignment>Top</VerticalAlignment>\
			<TextFitMode>None</TextFitMode>\
			<UseFullFontHeight>True</UseFullFontHeight>\
			<Verticalized>False</Verticalized>\
			<StyledText>\
				<Element>\
					<String>Bon Ton, Inc.</String>\
					<Attributes>\
						<Font Family="Arial" Size="6" Bold="True" Italic="False" Underline="False" Strikeout="False" />\
						<ForeColor Alpha="255" Red="255" Green="0" Blue="0" />\
					</Attributes>\
				</Element>\
			</StyledText>\
		</TextObject>\
		<Bounds X="705" Y="150" Width="782.887145996094" Height="120" />\
	</ObjectInfo>\
	<ObjectInfo>\
		<TextObject>\
			<Name>MerchID</Name>\
			<ForeColor Alpha="255" Red="0" Green="0" Blue="0" />\
			<BackColor Alpha="0" Red="255" Green="255" Blue="255" />\
			<LinkedObjectName></LinkedObjectName>\
			<Rotation>Rotation0</Rotation>\
			<IsMirrored>False</IsMirrored>\
			<IsVariable>False</IsVariable>\
			<HorizontalAlignment>Left</HorizontalAlignment>\
			<VerticalAlignment>Top</VerticalAlignment>\
			<TextFitMode>None</TextFitMode>\
			<UseFullFontHeight>True</UseFullFontHeight>\
			<Verticalized>False</Verticalized>\
			<StyledText>\
				<Element>\
					<String>Merch ID</String>\
					<Attributes>\
						<Font Family="Arial" Size="6" Bold="False" Italic="False" Underline="False" Strikeout="False" />\
						<ForeColor Alpha="255" Red="0" Green="0" Blue="0" />\
					</Attributes>\
				</Element>\
			</StyledText>\
		</TextObject>\
		<Bounds X="721.792053222656" Y="457.352048486695" Width="569.203552246094" Height="240" />\
	</ObjectInfo>\
	<ObjectInfo>\
		<TextObject>\
			<Name>VendorName</Name>\
			<ForeColor Alpha="255" Red="0" Green="0" Blue="0" />\
			<BackColor Alpha="0" Red="255" Green="255" Blue="255" />\
			<LinkedObjectName></LinkedObjectName>\
			<Rotation>Rotation0</Rotation>\
			<IsMirrored>False</IsMirrored>\
			<IsVariable>False</IsVariable>\
			<HorizontalAlignment>Left</HorizontalAlignment>\
			<VerticalAlignment>Top</VerticalAlignment>\
			<TextFitMode>None</TextFitMode>\
			<UseFullFontHeight>True</UseFullFontHeight>\
			<Verticalized>False</Verticalized>\
			<StyledText>\
				<Element>\
					<String>Vendor Name</String>\
					<Attributes>\
						<Font Family="Arial" Size="6" Bold="False" Italic="False" Underline="False" Strikeout="False" />\
						<ForeColor Alpha="255" Red="0" Green="0" Blue="0" />\
					</Attributes>\
				</Element>\
			</StyledText>\
		</TextObject>\
		<Bounds X="1533.11950683594" Y="450.134003373913" Width="959.203552246094" Height="120" />\
	</ObjectInfo>\
	<ObjectInfo>\
		<TextObject>\
			<Name>ItemDesc</Name>\
			<ForeColor Alpha="255" Red="0" Green="0" Blue="0" />\
			<BackColor Alpha="0" Red="255" Green="255" Blue="255" />\
			<LinkedObjectName></LinkedObjectName>\
			<Rotation>Rotation0</Rotation>\
			<IsMirrored>False</IsMirrored>\
			<IsVariable>False</IsVariable>\
			<HorizontalAlignment>Left</HorizontalAlignment>\
			<VerticalAlignment>Top</VerticalAlignment>\
			<TextFitMode>None</TextFitMode>\
			<UseFullFontHeight>True</UseFullFontHeight>\
			<Verticalized>False</Verticalized>\
			<StyledText>\
				<Element>\
					<String>Item Desc</String>\
					<Attributes>\
						<Font Family="Arial" Size="6" Bold="False" Italic="False" Underline="False" Strikeout="False" />\
						<ForeColor Alpha="255" Red="0" Green="0" Blue="0" />\
					</Attributes>\
				</Element>\
			</StyledText>\
		</TextObject>\
		<Bounds X="726.039794921875" Y="610.272397607789" Width="569.203552246094" Height="120" />\
	</ObjectInfo>\
	<ObjectInfo>\
		<TextObject>\
			<Name>Size</Name>\
			<ForeColor Alpha="255" Red="0" Green="0" Blue="0" />\
			<BackColor Alpha="0" Red="255" Green="255" Blue="255" />\
			<LinkedObjectName></LinkedObjectName>\
			<Rotation>Rotation0</Rotation>\
			<IsMirrored>False</IsMirrored>\
			<IsVariable>False</IsVariable>\
			<HorizontalAlignment>Left</HorizontalAlignment>\
			<VerticalAlignment>Top</VerticalAlignment>\
			<TextFitMode>None</TextFitMode>\
			<UseFullFontHeight>True</UseFullFontHeight>\
			<Verticalized>False</Verticalized>\
			<StyledText>\
				<Element>\
					<String>Size</String>\
					<Attributes>\
						<Font Family="Arial" Size="6" Bold="False" Italic="False" Underline="False" Strikeout="False" />\
						<ForeColor Alpha="255" Red="0" Green="0" Blue="0" />\
					</Attributes>\
				</Element>\
			</StyledText>\
		</TextObject>\
		<Bounds X="1541.61499023438" Y="586.925519211848" Width="569.203552246094" Height="120" />\
	</ObjectInfo>\
	<ObjectInfo>\
		<TextObject>\
			<Name>Figure</Name>\
			<ForeColor Alpha="255" Red="0" Green="0" Blue="0" />\
			<BackColor Alpha="0" Red="255" Green="255" Blue="255" />\
			<LinkedObjectName></LinkedObjectName>\
			<Rotation>Rotation0</Rotation>\
			<IsMirrored>False</IsMirrored>\
			<IsVariable>False</IsVariable>\
			<HorizontalAlignment>Left</HorizontalAlignment>\
			<VerticalAlignment>Top</VerticalAlignment>\
			<TextFitMode>None</TextFitMode>\
			<UseFullFontHeight>True</UseFullFontHeight>\
			<Verticalized>False</Verticalized>\
			<StyledText>\
				<Element>\
					<String>ON Figure</String>\
					<Attributes>\
						<Font Family="Arial" Size="6" Bold="False" Italic="False" Underline="False" Strikeout="False" />\
						<ForeColor Alpha="255" Red="0" Green="0" Blue="0" />\
					</Attributes>\
				</Element>\
			</StyledText>\
		</TextObject>\
		<Bounds X="3386.88061523438" Y="558.186566575129" Width="614.203552246094" Height="185.244360902255" />\
	</ObjectInfo>\
	<ObjectInfo>\
		<TextObject>\
			<Name>TEXT_3</Name>\
			<ForeColor Alpha="255" Red="0" Green="0" Blue="0" />\
			<BackColor Alpha="0" Red="255" Green="255" Blue="255" />\
			<LinkedObjectName></LinkedObjectName>\
			<Rotation>Rotation0</Rotation>\
			<IsMirrored>False</IsMirrored>\
			<IsVariable>False</IsVariable>\
			<HorizontalAlignment>Left</HorizontalAlignment>\
			<VerticalAlignment>Top</VerticalAlignment>\
			<TextFitMode>None</TextFitMode>\
			<UseFullFontHeight>True</UseFullFontHeight>\
			<Verticalized>False</Verticalized>\
			<StyledText>\
				<Element>\
					<String>Dept:</String>\
					<Attributes>\
						<Font Family="Arial" Size="6" Bold="True" Italic="False" Underline="False" Strikeout="False" />\
						<ForeColor Alpha="255" Red="255" Green="0" Blue="0" />\
					</Attributes>\
				</Element>\
			</StyledText>\
		</TextObject>\
		<Bounds X="719.535400390625" Y="784.431699365602" Width="611.681396484375" Height="120" />\
	</ObjectInfo>\
	<ObjectInfo>\
		<TextObject>\
			<Name>TEXT_4</Name>\
			<ForeColor Alpha="255" Red="0" Green="0" Blue="0" />\
			<BackColor Alpha="0" Red="255" Green="255" Blue="255" />\
			<LinkedObjectName></LinkedObjectName>\
			<Rotation>Rotation0</Rotation>\
			<IsMirrored>False</IsMirrored>\
			<IsVariable>False</IsVariable>\
			<HorizontalAlignment>Left</HorizontalAlignment>\
			<VerticalAlignment>Top</VerticalAlignment>\
			<TextFitMode>None</TextFitMode>\
			<UseFullFontHeight>True</UseFullFontHeight>\
			<Verticalized>False</Verticalized>\
			<StyledText>\
				<Element>\
					<String>Style:</String>\
					<Attributes>\
						<Font Family="Arial" Size="6" Bold="True" Italic="False" Underline="False" Strikeout="False" />\
						<ForeColor Alpha="255" Red="255" Green="0" Blue="0" />\
					</Attributes>\
				</Element>\
			</StyledText>\
		</TextObject>\
		<Bounds X="2873.16381835938" Y="797.17504653357" Width="611.681396484375" Height="168.75" />\
	</ObjectInfo>\
	<ObjectInfo>\
		<TextObject>\
			<Name>TEXT_5</Name>\
			<ForeColor Alpha="255" Red="0" Green="0" Blue="0" />\
			<BackColor Alpha="0" Red="255" Green="255" Blue="255" />\
			<LinkedObjectName></LinkedObjectName>\
			<Rotation>Rotation0</Rotation>\
			<IsMirrored>False</IsMirrored>\
			<IsVariable>False</IsVariable>\
			<HorizontalAlignment>Left</HorizontalAlignment>\
			<VerticalAlignment>Top</VerticalAlignment>\
			<TextFitMode>None</TextFitMode>\
			<UseFullFontHeight>True</UseFullFontHeight>\
			<Verticalized>False</Verticalized>\
			<StyledText>\
				<Element>\
					<String>Ad Nbr:</String>\
					<Attributes>\
						<Font Family="Arial" Size="6" Bold="True" Italic="False" Underline="False" Strikeout="False" />\
						<ForeColor Alpha="255" Red="255" Green="0" Blue="0" />\
					</Attributes>\
				</Element>\
			</StyledText>\
		</TextObject>\
		<Bounds X="715.28759765625" Y="971.59985122107" Width="470.542022705078" Height="120" />\
	</ObjectInfo>\
	<ObjectInfo>\
		<TextObject>\
			<Name>Dept</Name>\
			<ForeColor Alpha="255" Red="0" Green="0" Blue="0" />\
			<BackColor Alpha="0" Red="255" Green="255" Blue="255" />\
			<LinkedObjectName></LinkedObjectName>\
			<Rotation>Rotation0</Rotation>\
			<IsMirrored>False</IsMirrored>\
			<IsVariable>False</IsVariable>\
			<HorizontalAlignment>Left</HorizontalAlignment>\
			<VerticalAlignment>Top</VerticalAlignment>\
			<TextFitMode>None</TextFitMode>\
			<UseFullFontHeight>True</UseFullFontHeight>\
			<Verticalized>False</Verticalized>\
			<StyledText>\
				<Element>\
					<String>XXX</String>\
					<Attributes>\
						<Font Family="Arial" Size="6" Bold="False" Italic="False" Underline="False" Strikeout="False" />\
						<ForeColor Alpha="255" Red="0" Green="0" Blue="0" />\
					</Attributes>\
				</Element>\
			</StyledText>\
		</TextObject>\
		<Bounds X="1169.31985175341" Y="801.655387247416" Width="569.203552246094" Height="120" />\
	</ObjectInfo>\
	<ObjectInfo>\
		<TextObject>\
			<Name>Color</Name>\
			<ForeColor Alpha="255" Red="0" Green="0" Blue="0" />\
			<BackColor Alpha="0" Red="255" Green="255" Blue="255" />\
			<LinkedObjectName></LinkedObjectName>\
			<Rotation>Rotation0</Rotation>\
			<IsMirrored>False</IsMirrored>\
			<IsVariable>False</IsVariable>\
			<HorizontalAlignment>Left</HorizontalAlignment>\
			<VerticalAlignment>Top</VerticalAlignment>\
			<TextFitMode>None</TextFitMode>\
			<UseFullFontHeight>True</UseFullFontHeight>\
			<Verticalized>False</Verticalized>\
			<StyledText>\
				<Element>\
					<String>COLOR</String>\
					<Attributes>\
						<Font Family="Arial" Size="6" Bold="False" Italic="False" Underline="False" Strikeout="False" />\
						<ForeColor Alpha="255" Red="0" Green="0" Blue="0" />\
					</Attributes>\
				</Element>\
			</StyledText>\
		</TextObject>\
		<Bounds X="1956.43811035156" Y="799.96264418982" Width="569.203552246094" Height="120" />\
	</ObjectInfo>\
	<ObjectInfo>\
		<TextObject>\
			<Name>AdNoDesc</Name>\
			<ForeColor Alpha="255" Red="0" Green="0" Blue="0" />\
			<BackColor Alpha="0" Red="255" Green="255" Blue="255" />\
			<LinkedObjectName></LinkedObjectName>\
			<Rotation>Rotation0</Rotation>\
			<IsMirrored>False</IsMirrored>\
			<IsVariable>False</IsVariable>\
			<HorizontalAlignment>Left</HorizontalAlignment>\
			<VerticalAlignment>Top</VerticalAlignment>\
			<TextFitMode>None</TextFitMode>\
			<UseFullFontHeight>True</UseFullFontHeight>\
			<Verticalized>False</Verticalized>\
			<StyledText>\
				<Element>\
					<String>01- Desc</String>\
					<Attributes>\
						<Font Family="Arial" Size="6" Bold="False" Italic="False" Underline="False" Strikeout="False" />\
						<ForeColor Alpha="255" Red="0" Green="0" Blue="0" />\
					</Attributes>\
				</Element>\
			</StyledText>\
		</TextObject>\
		<Bounds X="1213.0751953125" Y="970.139646142945" Width="569.203552246094" Height="120" />\
	</ObjectInfo>\
	<ObjectInfo>\
		<TextObject>\
			<Name>Style</Name>\
			<ForeColor Alpha="255" Red="0" Green="0" Blue="0" />\
			<BackColor Alpha="0" Red="255" Green="255" Blue="255" />\
			<LinkedObjectName></LinkedObjectName>\
			<Rotation>Rotation0</Rotation>\
			<IsMirrored>False</IsMirrored>\
			<IsVariable>False</IsVariable>\
			<HorizontalAlignment>Left</HorizontalAlignment>\
			<VerticalAlignment>Top</VerticalAlignment>\
			<TextFitMode>None</TextFitMode>\
			<UseFullFontHeight>True</UseFullFontHeight>\
			<Verticalized>False</Verticalized>\
			<StyledText>\
				<Element>\
					<String>XXXXXXXXXXXXX</String>\
					<Attributes>\
						<Font Family="Arial" Size="6" Bold="False" Italic="False" Underline="False" Strikeout="False" />\
						<ForeColor Alpha="255" Red="0" Green="0" Blue="0" />\
					</Attributes>\
				</Element>\
			</StyledText>\
		</TextObject>\
		<Bounds X="3271.7919921875" Y="802.75024184607" Width="1042.45910828096" Height="120" />\
	</ObjectInfo>\
	<ObjectInfo>\
		<TextObject>\
			<Name>TEXT__6</Name>\
			<ForeColor Alpha="255" Red="0" Green="0" Blue="0" />\
			<BackColor Alpha="0" Red="255" Green="255" Blue="255" />\
			<LinkedObjectName></LinkedObjectName>\
			<Rotation>Rotation0</Rotation>\
			<IsMirrored>False</IsMirrored>\
			<IsVariable>False</IsVariable>\
			<HorizontalAlignment>Left</HorizontalAlignment>\
			<VerticalAlignment>Top</VerticalAlignment>\
			<TextFitMode>None</TextFitMode>\
			<UseFullFontHeight>True</UseFullFontHeight>\
			<Verticalized>False</Verticalized>\
			<StyledText>\
				<Element>\
					<String>Sys Page:</String>\
					<Attributes>\
						<Font Family="Arial" Size="6" Bold="True" Italic="False" Underline="False" Strikeout="False" />\
						<ForeColor Alpha="255" Red="255" Green="0" Blue="0" />\
					</Attributes>\
				</Element>\
			</StyledText>\
		</TextObject>\
		<Bounds X="713.827453613281" Y="1157.04229506873" Width="606.238952636719" Height="131.25" />\
	</ObjectInfo>\
	<ObjectInfo>\
		<TextObject>\
			<Name>TEXT__7</Name>\
			<ForeColor Alpha="255" Red="0" Green="0" Blue="0" />\
			<BackColor Alpha="0" Red="255" Green="255" Blue="255" />\
			<LinkedObjectName></LinkedObjectName>\
			<Rotation>Rotation0</Rotation>\
			<IsMirrored>False</IsMirrored>\
			<IsVariable>False</IsVariable>\
			<HorizontalAlignment>Left</HorizontalAlignment>\
			<VerticalAlignment>Top</VerticalAlignment>\
			<TextFitMode>None</TextFitMode>\
			<UseFullFontHeight>True</UseFullFontHeight>\
			<Verticalized>False</Verticalized>\
			<StyledText>\
				<Element>\
					<String>Page:</String>\
					<Attributes>\
						<Font Family="Arial" Size="6" Bold="True" Italic="False" Underline="False" Strikeout="False" />\
						<ForeColor Alpha="255" Red="255" Green="0" Blue="0" />\
					</Attributes>\
				</Element>\
			</StyledText>\
		</TextObject>\
		<Bounds X="1797.01330566406" Y="1148.54668959998" Width="399.292022705078" Height="120" />\
	</ObjectInfo>\
	<ObjectInfo>\
		<TextObject>\
			<Name>SysPage</Name>\
			<ForeColor Alpha="255" Red="0" Green="0" Blue="0" />\
			<BackColor Alpha="0" Red="255" Green="255" Blue="255" />\
			<LinkedObjectName></LinkedObjectName>\
			<Rotation>Rotation0</Rotation>\
			<IsMirrored>False</IsMirrored>\
			<IsVariable>False</IsVariable>\
			<HorizontalAlignment>Left</HorizontalAlignment>\
			<VerticalAlignment>Top</VerticalAlignment>\
			<TextFitMode>None</TextFitMode>\
			<UseFullFontHeight>True</UseFullFontHeight>\
			<Verticalized>False</Verticalized>\
			<StyledText>\
				<Element>\
					<String>X</String>\
					<Attributes>\
						<Font Family="Arial" Size="6" Bold="False" Italic="False" Underline="False" Strikeout="False" />\
						<ForeColor Alpha="255" Red="0" Green="0" Blue="0" />\
					</Attributes>\
				</Element>\
			</StyledText>\
		</TextObject>\
		<Bounds X="1355.84069824219" Y="1175.32769545935" Width="152.953536987305" Height="120" />\
	</ObjectInfo>\
	<ObjectInfo>\
		<TextObject>\
			<Name>Page</Name>\
			<ForeColor Alpha="255" Red="0" Green="0" Blue="0" />\
			<BackColor Alpha="0" Red="255" Green="255" Blue="255" />\
			<LinkedObjectName></LinkedObjectName>\
			<Rotation>Rotation0</Rotation>\
			<IsMirrored>False</IsMirrored>\
			<IsVariable>False</IsVariable>\
			<HorizontalAlignment>Left</HorizontalAlignment>\
			<VerticalAlignment>Top</VerticalAlignment>\
			<TextFitMode>None</TextFitMode>\
			<UseFullFontHeight>True</UseFullFontHeight>\
			<Verticalized>False</Verticalized>\
			<StyledText>\
				<Element>\
					<String>X</String>\
					<Attributes>\
						<Font Family="Arial" Size="6" Bold="False" Italic="False" Underline="False" Strikeout="False" />\
						<ForeColor Alpha="255" Red="0" Green="0" Blue="0" />\
					</Attributes>\
				</Element>\
			</StyledText>\
		</TextObject>\
		<Bounds X="2195.6416015625" Y="1166.8652931156" Width="569.203552246094" Height="120" />\
	</ObjectInfo>\
	<ObjectInfo>\
		<TextObject>\
			<Name>TEXT___4</Name>\
			<ForeColor Alpha="255" Red="0" Green="0" Blue="0" />\
			<BackColor Alpha="0" Red="255" Green="255" Blue="255" />\
			<LinkedObjectName></LinkedObjectName>\
			<Rotation>Rotation0</Rotation>\
			<IsMirrored>False</IsMirrored>\
			<IsVariable>False</IsVariable>\
			<HorizontalAlignment>Left</HorizontalAlignment>\
			<VerticalAlignment>Top</VerticalAlignment>\
			<TextFitMode>None</TextFitMode>\
			<UseFullFontHeight>True</UseFullFontHeight>\
			<Verticalized>False</Verticalized>\
			<StyledText>\
				<Element>\
					<String>UPC</String>\
					<Attributes>\
						<Font Family="Arial" Size="6" Bold="True" Italic="False" Underline="False" Strikeout="False" />\
						<ForeColor Alpha="255" Red="255" Green="0" Blue="0" />\
					</Attributes>\
				</Element>\
			</StyledText>\
		</TextObject>\
		<Bounds X="712.367248535156" Y="1350.98040542029" Width="321.238952636719" Height="120" />\
	</ObjectInfo>\
	<ObjectInfo>\
		<TextObject>\
			<Name>UPC</Name>\
			<ForeColor Alpha="255" Red="0" Green="0" Blue="0" />\
			<BackColor Alpha="0" Red="255" Green="255" Blue="255" />\
			<LinkedObjectName></LinkedObjectName>\
			<Rotation>Rotation0</Rotation>\
			<IsMirrored>False</IsMirrored>\
			<IsVariable>False</IsVariable>\
			<HorizontalAlignment>Left</HorizontalAlignment>\
			<VerticalAlignment>Top</VerticalAlignment>\
			<TextFitMode>None</TextFitMode>\
			<UseFullFontHeight>True</UseFullFontHeight>\
			<Verticalized>False</Verticalized>\
			<StyledText>\
				<Element>\
					<String>XXXXXXXXXXXXX</String>\
					<Attributes>\
						<Font Family="Arial" Size="6" Bold="False" Italic="False" Underline="False" Strikeout="False" />\
						<ForeColor Alpha="255" Red="0" Green="0" Blue="0" />\
					</Attributes>\
				</Element>\
			</StyledText>\
		</TextObject>\
		<Bounds X="1048.2412109375" Y="1345.77020034216" Width="1497.47790527344" Height="120" />\
	</ObjectInfo>\
	<ObjectInfo>\
		<BarcodeObject>\
			<Name>BARCODE</Name>\
			<ForeColor Alpha="255" Red="0" Green="0" Blue="0" />\
			<BackColor Alpha="0" Red="255" Green="255" Blue="255" />\
			<LinkedObjectName></LinkedObjectName>\
			<Rotation>Rotation0</Rotation>\
			<IsMirrored>False</IsMirrored>\
			<IsVariable>True</IsVariable>\
			<Text>12345</Text>\
			<Type>Code39</Type>\
			<Size>Medium</Size>\
			<TextPosition>None</TextPosition>\
			<TextFont Family="Arial" Size="8" Bold="False" Italic="False" Underline="False" Strikeout="False" />\
			<CheckSumFont Family="Arial" Size="8" Bold="False" Italic="False" Underline="False" Strikeout="False" />\
			<TextEmbedding>None</TextEmbedding>\
			<ECLevel>0</ECLevel>\
			<HorizontalAlignment>Center</HorizontalAlignment>\
			<QuietZonesPadding Left="0" Top="0" Right="0" Bottom="0" />\
		</BarcodeObject>\
		<Bounds X="1466.68421052632" Y="150" Width="2172.63157894737" Height="120" />\
	</ObjectInfo>\
</DieCutLabel>';

    // prints printers information
    function print(printerName, merchID) {
        var printers = dymo.label.framework.getPrinters();
        var printer = printers[printerName];
        if (!printer) {
            alert("Printer '" + printerName + "' not found");
            return;
        }

        // select label layout/template based on printer type
        var labelXml;
        if (printer.printerType == "LabelWriterPrinter")
            labelXml = dieCutLabelLayout;
        else {
            alert("Unsupported printer type");
            throw "Unsupported printer type";
        }

        // create label set to print printers' data
        var labelSetBuilder = new dymo.label.framework.LabelSetBuilder();
        for (var i = 0; i < printers.length; i++) {
            var printer = printers[i];

            // process each printer info as a separate label
            var record = labelSetBuilder.addRecord();

            record.setTextMarkup("BARCODE", "653");
            record.setTextMarkup("MerchID", merchID);
            record.setTextMarkup("VendorName", "Test Vendor");
            record.setTextMarkup("ItemDesc", "Testing Print Label");
            record.setTextMarkup("Size", "MED");
            record.setTextMarkup("Figure", "Test");
            record.setTextMarkup("Dept", "14");
            record.setTextMarkup("Color", "White");
            record.setTextMarkup("AdNoDesc", "Test Ad");
            record.setTextMarkup("Style", "Formals");
            record.setTextMarkup("SysPage", "3");
            record.setTextMarkup("Page", "4");
            record.setTextMarkup("UPC", "12343214");
        }

        // finally print label with default printing parameters
        dymo.label.framework.printLabel(printerName, "", labelXml, labelSetBuilder);
    }

    // called when the document completly loaded
    function onload() {
        var printButton = document.getElementById('printButton');

        // testhandler.ashx?StrMethodName=STUFF
        // setup event handlers
        printButton.onclick = function () {
            alert("User clicked");
            var jqResponse = $.post("testhandler.ashx",
                        {
                            MerchID:"43123"
                        }, 
                        function (data, status) {
                            alert("Data: " + data + "\nStatus: " + status);
                            print("DYMO LabelWriter 450", data);
                        });
            //            print("DYMO LabelWriter 450");
            //            .done(function() { alert("Print Label"); })
            //            .fail(function() { alert("Error"); })
            //            .always(function() { alert("Finished"); });
            alert("Server side call completed.");
        }
    };

    function loadLabels() {
        $.get("UPC.label", function (labelXml) {
            addressLabel = dymo.label.framework.openLabelXml(labelXml);
        }, "text");
    }

    // register onload event
    if (window.addEventListener)
        window.addEventListener("load", onload, false);
    else if (window.attachEvent)
        window.attachEvent("onload", onload);
    else
        window.onload = onload;

} ());