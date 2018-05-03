
"use strict";

// Optional. You will see this name in eg. 'ps' or 'top' command
process.title = 'node-chat';

// Port where we'll run the websocket server
var webSocketsServerPort = 1510;

// websocket and http servers
var webSocketServer = require('websocket').server;
var http = require('http');

var responses =
{
    config: {
        "MessageType": "Response",
        "Device": "Server",
        "ResponseType": "Data",
        "ServerIPAddress": "10.101.235.60",
        "ServerName": "POS048030",
        "StoreNumber": 48,
        "TerminalNumber": 30,
        "Timestamp": "2016-09-16 16:02:56",
        "StatusCode": "SVR_DATA_INFO",
        "StatusText": "The PMM server info was returned successfully.",
        "MSRFunctional": "False",
        "UseEPS": "True",
        "PrinterFunctional": "False",
        "ScannerFunctional": "True",
        "ServerModel": "7402-1254-8801",
        "ServerOS": "POSReady 2009",
        "SoftwareVersion": "1.16.803.1"
    },
    msrSCSClaimed: {
        "MessageType": "Response",
        "Device": "MSR",
        "ResponseType": "Success",
        "ServerIPAddress": "10.162.218.60",
        "ServerName": "POS357104",
        "StoreNumber": 48,
        "TerminalNumber": 30,
        "Timestamp": "2016-10-12 15:58:39",
        "StatusCode": "MSR_SCS_CLAIMED",
        "StatusText": "The MSR was successfully claimed."
    },

    msrSCSEnabled: {
        "MessageType": "Response",
        "Device": "MSR",
        "ResponseType": "Success",
        "ServerIPAddress": "10.162.218.60",
        "ServerName": "POS357104",
        "StoreNumber": 48,
        "TerminalNumber": 30,
        "Timestamp": "2016-10-12 16:04:30",
        "StatusCode": "MSR_SCS_ENABLED",
        "StatusText": "The MSR was successfully enabled for BankCard input."
    },

    msrDataVBankcard: {
        "MessageType": "Response",
        "Device": "MSR",
        "ResponseType": "Data",
        "ServerIPAddress": "10.101.235.62",
        "ServerName": "POS310001",
        "StoreNumber": 48,
        "TerminalNumber": 30,
        "Timestamp": "2016-08-19 12:06:45",
        "StatusCode": "MSR_DATA_SWIPED",
        "StatusText": "A card was successfully swiped.  The data returned is included in the response message, and the MSR has been disabled.",
        "CardSwiped": "True",
        "AccountNumberIndicator": "V",
        "AccountNumber": "2353723337270000",
        "AccountNumberLast4": "0000",
        "CardType": "Discover",
        "FirstName": "JCB",
        "MiddleInitial": "",
        "LastName": "TESTCARD",
        "ExpirationDate": "1219",
        "Track1Data": "**************",
        "Track2Data": "353011******0000=********************",
        "Track3Data": ""
    },
    msrDataCPlcc: {
        "MessageType": "Response",
        "Device": "MSR",
        "ResponseType": "Data",
        "ServerIPAddress": "10.101.235.62",
        "ServerName": "POS310001",
        "StoreNumber": 48,
        "TerminalNumber": 30,
        "Timestamp": "2016-08-24 08:51:24",
        "StatusCode": "MSR_DATA_SWIPED",
        "StatusText": "A card was successfully swiped.  The data returned is included in the response message, and the MSR has been disabled.",
        "CardSwiped": "True",
        "AccountNumberIndicator": "C",
        "AccountNumber": "2117010000000013",
        "AccountNumberLast4": "0013",
        "CardType": "PLCC",
        "FirstName": "BONTON",
        "MiddleInitial": "",
        "LastName": "BONTONONE",
        "ExpirationDate": "",
        "Track1Data": "",
        "Track2Data": "211720*******************************",
        "Track3Data": ""
    },
    msrERRCanceled: {
        "MessageType": "Response",
        "Device": "MSR",
        "ResponseType": "Error",
        "ServerIPAddress": "10.101.235.62",
        "ServerName": "POS310010",
        "StoreNumber": 48,
        "TerminalNumber": 30,
        "Timestamp": "2016-11-08 10:50:00",
        "StatusCode": "MSR_ERR_BANK_CUST_CANCEL",
        "StatusText": "The bank card swipe was cancelled by the customer."
    },
    msrERRInvalidType: {
        "MessageType": "Response",
        "Device": "MSR",
        "ResponseType": "Error",
        "ServerIPAddress": "10.101.235.62",
        "ServerName": "POS310010",
        "StoreNumber": 48,
        "TerminalNumber": 30,
        "Timestamp": "2016-11-08 10:48:25",
        "StatusCode": "MSR_ERR_INVALID_TYPE",
        "StatusText": "The card swiped or manually entered is of an invalid card type."
    },
    msrERRDisable: {
        "MessageType": "Response",
        "Device": "MSR",
        "ResponseType": "Error",
        "ServerIPAddress": "10.100.130.107",
        "ServerName": "POS130011",
        "StoreNumber": 48,
        "TerminalNumber": 30,
        "Timestamp": "2016-11-03 11:48:02",
        "StatusCode": "MSR_ERR_DISABLE",
        "StatusText": "The MSR cannot be disabled.  Error: \"Card swipe has already occurred\""
    },
    msrERRNotEnabled: {
        "MessageType": "Response",
        "Device": "MSR",
        "ResponseType": "Error",
        "ServerIPAddress": "10.101.235.62",
        "ServerName": "POS310001",
        "StoreNumber": 48,
        "TerminalNumber": 30,
        "Timestamp": "2016-08-19 12:06:45",
        "StatusCode": "MSR_ERR_NOT_ENABLED",
        "StatusText": "The MSR has not been enabled."
    },
    msrERRNotFunc: {
        "MessageType": "Response",
        "Device": "MSR",
        "ResponseType": "Error",
        "ServerIPAddress": "10.162.218.60",
        "ServerName": "POS357104",
        "StoreNumber": 48,
        "TerminalNumber": 30,
        "Timestamp": "2016-10-27 12:39:09",
        "StatusCode": "MSR_ERR_NOT_FUNCTIONAL",
        "StatusText": "The MSR is not functional."
    },
    bopisSCSEnabled: {
        "MessageType": "Response",
        "Device": "PaymentTerminal",
        "ResponseType": "Success",
        "ServerIPAddress": "10.162.218.60",
        "ServerName": "POS357104",
        "StoreNumber": 48,
        "TerminalNumber": 30,
        "Timestamp": "2016-10-12 16:04:30",
        "StatusCode": "PMT_SCS_ENABLED",
        "StatusText": "The payment terminal was successfully enabled for signature capture."
    },
    bopisSCSClaimed: {
        "MessageType": "Response",
        "Device": "PaymentTerminal",
        "ResponseType": "Success",
        "ServerIPAddress": "10.162.218.60",
        "ServerName": "POS357104",
        "StoreNumber": 48,
        "TerminalNumber": 30,
        "Timestamp": "2016-10-12 16:04:30",
        "StatusCode": "PMT_SCS_CLAIMED",
        "StatusText": "The payment terminal was successfully claimed."
    },
    bopisSCSReleased: {
        "MessageType": "Response",
        "Device": "PaymentTerminal",
        "ResponseType": "Success",
        "ServerIPAddress": "10.162.218.60",
        "ServerName": "POS357104",
        "StoreNumber": 48,
        "TerminalNumber": 30,
        "Timestamp": "2016-10-12 16:04:30",
        "StatusCode": "PMT_SCS_RELEASED",
        "StatusText": "The payment terminal was successfully released."
    },
    bopisSCSData: {
        "MessageType": "Response",
        "Device": "PaymentTerminal",
        "ResponseType": "Success",
        "ServerIPAddress": "10.162.218.60",
        "ServerName": "POS357104",
        "StoreNumber": 48,
        "TerminalNumber": 30,
        "Timestamp": "2016-10-12 16:04:30",
        "StatusCode": "PMT_SCS_SIGNATURE",
        "StatusText": "Signature captured successfully, and saved in POS transaction for data collect."
    },
    bopisERRNotFunctional: {
        "MessageType": "Response",
        "Device": "PaymentTerminal",
        "ResponseType": "Error",
        "ServerIPAddress": "10.102.171.76",
        "ServerName": "POS519112",
        "StoreNumber": 48,
        "TerminalNumber": 30,
        "Timestamp": "2016-11-11 08:16:22",
        "StatusCode": "PMT_ERR_NOT_FUNCTIONAL",
        "StatusText": "The payment terminal is not functional."
    },
    bopisERRAlreadyClaimed: {
        "MessageType": "Response",
        "Device": "PaymentTerminal",
        "ResponseType": "Error",
        "ServerIPAddress": "10.102.171.76",
        "ServerName": "POS519112",
        "StoreNumber": 48,
        "TerminalNumber": 30,
        "Timestamp": "2016-11-11 08:16:22",
        "StatusCode": "PMT_ERR_CLAIMED",
        "StatusText": "The payment terminal has already been claimed."
    },
    bopisERRClaimedOther: {
        "MessageType": "Response",
        "Device": "PaymentTerminal",
        "ResponseType": "Error",
        "ServerIPAddress": "10.102.171.76",
        "ServerName": "POS519112",
        "StoreNumber": 48,
        "TerminalNumber": 30,
        "Timestamp": "2016-11-11 08:16:22",
        "StatusCode": "PMT_ERR_CLAIMED_OTHER",
        "StatusText": "The payment terminal has already been claimed by another socket:..."
    },
    bopisERRRelease: {
        "MessageType": "Response",
        "Device": "PaymentTerminal",
        "ResponseType": "Error",
        "ServerIPAddress": "10.102.171.76",
        "ServerName": "POS519112",
        "StoreNumber": 48,
        "TerminalNumber": 30,
        "Timestamp": "2016-11-11 08:16:22",
        "StatusCode": "PMT_ERR_RELEASE",
        "StatusText": "The payment terminal cannot be released.  Error:..."
    },
    bopisERRNotClaimed: {
        "MessageType": "Response",
        "Device": "PaymentTerminal",
        "ResponseType": "Error",
        "ServerIPAddress": "10.102.171.76",
        "ServerName": "POS519112",
        "StoreNumber": 48,
        "TerminalNumber": 30,
        "Timestamp": "2016-11-11 08:16:22",
        "StatusCode": "PMT_ERR_NOT_CLAIMED",
        "StatusText": "The payment terminal has not been claimed."
    },
    bopisERRAlreadyEnabled: {
        "MessageType": "Response",
        "Device": "PaymentTerminal",
        "ResponseType": "Error",
        "ServerIPAddress": "10.102.171.76",
        "ServerName": "POS519112",
        "StoreNumber": 48,
        "TerminalNumber": 30,
        "Timestamp": "2016-11-11 08:16:22",
        "StatusCode": "PMT_ERR_ENABLED",
        "StatusText": "The payment terminal has already been enabled for MSR or signature input."
    },
    bopisERRCannotEnable: {
        "MessageType": "Response",
        "Device": "PaymentTerminal",
        "ResponseType": "Error",
        "ServerIPAddress": "10.102.171.76",
        "ServerName": "POS519112",
        "StoreNumber": 48,
        "TerminalNumber": 30,
        "Timestamp": "2016-11-11 08:16:22",
        "StatusCode": "PMT_ERR_ENABLE",
        "StatusText": "The payment terminal cannot be enabled.  Error: ..."
    },
    bopisERRValidation: {
        "MessageType": "Response",
        "Device": "PaymentTerminal",
        "ResponseType": "Error",
        "ServerIPAddress": "10.102.171.76",
        "ServerName": "POS519112",
        "StoreNumber": 48,
        "TerminalNumber": 30,
        "Timestamp": "2016-11-11 08:16:22",
        "StatusCode": "PMT_ERR_VALIDATION",
        "StatusText": "The payment terminal cannot be enabled for signature capture.  Validation error: ..."
    },
    bopisERRUnexpected: {
        "MessageType": "Response",
        "Device": "PaymentTerminal",
        "ResponseType": "Error",
        "ServerIPAddress": "10.100.135.125",
        "ServerName": "POS135005",
        "StoreNumber": 48,
        "TerminalNumber": 30,
        "Timestamp": "2016-11-07 13:39:07",
        "StatusCode": "PMT_ERR_UNEXPECTED",
        "StatusText": "An unexpected error occurred while attempting to create the signature POS transaction for data collect."
    },
    bopisERRCustomerCanceled: {
        "MessageType": "Response",
        "Device": "PaymentTerminal",
        "ResponseType": "Error",
        "ServerIPAddress": "10.102.171.76",
        "ServerName": "POS519112",
        "StoreNumber": 48,
        "TerminalNumber": 30,
        "Timestamp": "2016-11-11 08:16:22",
        "StatusCode": "PMT_ERR_SIG_CUST_CANCEL",
        "StatusText": "The signature capture was cancelled by the customer."
    },
    bopisERRShutdown: {
        "MessageType": "Response",
        "Device": "PaymentTerminal",
        "ResponseType": "Error",
        "ServerIPAddress": "10.102.171.76",
        "ServerName": "POS519112",
        "StoreNumber": 48,
        "TerminalNumber": 30,
        "Timestamp": "2016-11-11 08:16:22",
        "StatusCode": "PMT_ERR_SHUTDOWN",
        "StatusText": "The payment terminal claim was released... PMM is shutting down."
    },
    bopisERRSettings: {
        "MessageType": "Response",
        "Device": "PaymentTerminal",
        "ResponseType": "Error",
        "ServerIPAddress": "10.102.171.76",
        "ServerName": "POS519112",
        "StoreNumber": 48,
        "TerminalNumber": 30,
        "Timestamp": "2016-11-11 08:16:22",
        "StatusCode": "PMT_ERR_SETTINGS",
        "StatusText": "The payment terminal claim was released due to an MSR or PaymentTerminal settings change at the server."
    },
    scannerSCSClaimed: {
        "MessageType": "Response",
        "Device": "Scanner",
        "ResponseType": "Success",
        "ServerIPAddress": "10.101.235.60",
        "ServerName": "POS048030",
        "StoreNumber": 48,
        "TerminalNumber": 30,
        "Timestamp": "2016-07-29 10:52:34",
        "StatusCode": "SCN_SCS_CLAIMED",
        "StatusText": "The scanner was successfully claimed."
    },
    scannerSCSEnabled: {
        "MessageType": "Response",
        "Device": "Scanner",
        "ResponseType": "Success",
        "ServerIPAddress": "10.101.235.60",
        "ServerName": "POS048030",
        "StoreNumber": 48,
        "TerminalNumber": 30,
        "Timestamp": "2016-07-06 12:06:49",
        "StatusCode": "SCN_SCS_ENABLED",
        "StatusText": "The scanner was successfully enabled."
    },
    scannerSCSDisabled: {
        "MessageType": "Response",
        "Device": "Scanner",
        "ResponseType": "Success",
        "ServerIPAddress": "10.101.235.60",
        "ServerName": "POS048030",
        "StoreNumber": 48,
        "TerminalNumber": 30,
        "Timestamp": "2016-07-06 12:06:49",
        "StatusCode": "SCN_SCS_DISABLED",
        "StatusText": "The scanner was successfully disabled."
    },
    scannerDataUpc: {
        "MessageType": "Response",
        "Device": "Scanner",
        "ResponseType": "Data",
        "ServerIPAddress": "10.101.235.62",
        "ServerName": "POS310001",
        "StoreNumber": 48,
        "TerminalNumber": 30,
        "Timestamp": "2016-08-19 12:03:36",
        "StatusCode": "SCN_DATA_SCANNED",
        "StatusText": "A barcode was successfully scanned.  The data returned is included in the response message.",
        "BarcodeType": 8,
        "BarcodeTypeText": "UPC-A",
        "BarcodeData": "735728528788"
    },
    scannerDataTempPLCC: {
        "MessageType": "Response",
        "Device": "Scanner",
        "ResponseType": "Data",
        "ServerIPAddress": "10.101.235.62",
        "ServerName": "POS310001",
        "StoreNumber": 48,
        "TerminalNumber": 30,
        "Timestamp": "2017-04-05 14:34:04",
        "StatusCode": "SCN_DATA_SCANNED",
        "StatusText": "A barcode was successfully scanned.  The data returned is included in the response message.",
        "BarcodeType": 17,
        "BarcodeTypeText": "PDF-417",
        "BarcodeData": "2117010000000013"
    },
    scannerERRInvalidStore:{
        "MessageType":"Response",
        "Device":"Scanner",
        "ResponseType":"Error",
        "ServerIPAddress":"192.168.1.9",
        "ServerName":"POS311101",
        "StoreNumber":311,
        "TerminalNumber":101,
        "Timestamp":"2017-04-05 14:46:28",
        "StatusCode":"SCN_ERR_INVALID_STORE",
        "StatusText":"The temporary charge card is invalid - invalid store number"
    },
    scannerERRExpiredCard:{
        "MessageType":"Response",
        "Device":"Scanner",
        "ResponseType":"Error",
        "ServerIPAddress":"192.168.1.9",
        "ServerName":"POS310101",
        "StoreNumber":310,
        "TerminalNumber":101,
        "Timestamp":"2017-04-05 14:33:49",
        "StatusCode":"SCN_ERR_EXPIRED",
        "StatusText":"The temporary charge card is invalid - expired"
    },
    scannerERRInvalidBarcode:{
        "MessageType":"Response",
        "Device":"Scanner",
        "ResponseType":"Error",
        "ServerIPAddress":"192.168.1.9",
        "ServerName":"POS310101",
        "StoreNumber":310,
        "TerminalNumber":101,
        "Timestamp":"2017-04-05 14:33:35",
        "StatusCode":"SCN_ERR_INVALID_BARCODE",
        "StatusText":"The barcode scanned is invalid."
    },
    scannerERRAlreadyClaimed: {
        "MessageType": "Response",
        "Device": "Scanner",
        "ResponseType": "Error",
        "ServerIPAddress": "10.101.235.60",
        "ServerName": "POS048030",
        "StoreNumber": 48,
        "TerminalNumber": 30,
        "Timestamp": "2016-07-29 10:52:34",
        "StatusCode": "SCN_ERR_CLAIMED",
        "StatusText": "The scanner has already been claimed."
    },
    scannerERRNotClaimed: {
        "MessageType": "Response",
        "Device": "Scanner",
        "ResponseType": "Error",
        "ServerIPAddress": "10.101.235.60",
        "ServerName": "POS048030",
        "StoreNumber": 48,
        "TerminalNumber": 30,
        "Timestamp": "2016-07-29 10:52:34",
        "StatusCode": "SCN_ERR_NOT_CLAIMED",
        "StatusText": "'The scanner has not been claimed."
    },
    scannerERRNotEnabled: {
        "MessageType": "Response",
        "Device": "Scanner",
        "ResponseType": "Error",
        "ServerIPAddress": "10.101.235.62",
        "ServerName": "POS310001",
        "StoreNumber": 310,
        "TerminalNumber": 1,
        "Timestamp": "2016-08-19 12:04:21",
        "StatusCode": "SCN_ERR_NOT_ENABLED",
        "StatusText": "The scanner has not been enabled."
    },
    printerSCSClaimed: {
        "MessageType": "Response",
        "Device": "Printer",
        "ResponseType": "Success",
        "ServerIPAddress": "10.101.235.60",
        "ServerName": "POS048030",
        "StoreNumber": 48,
        "TerminalNumber": 30,
        "Timestamp": "2016-08-12 14:16:22",
        "StatusCode": "PRT_SCS_CLAIMED",
        "StatusText": "The printer was successfully claimed."
    },
    printerSCSPrinted: {
        "MessageType": "Response",
        "Device": "Printer",
        "ResponseType": "Success",
        "ServerIPAddress": "10.101.235.60",
        "ServerName": "POS048030",
        "StoreNumber": 48,
        "TerminalNumber": 30,
        "Timestamp": "2016-08-12 14:16:25",
        "StatusCode": "PRT_SCS_PRINTED",
        "StatusText": "The order was successfully retrieved and printed."
    },
    printerSCSReleased: {
        "MessageType": "Response",
        "Device": "Printer",
        "ResponseType": "Success",
        "ServerIPAddress": "10.101.235.60",
        "ServerName": "POS048030",
        "StoreNumber": 48,
        "TerminalNumber": 30,
        "Timestamp": "2016-08-12 14:16:25",
        "StatusCode": "PRT_SCS_RELEASED",
        "StatusText": "The printer was successfully released."
    },
    printerERRNotFunc: {
        "MessageType": "Response",
        "Device": "Printer",
        "ResponseType": "Error",
        "ServerIPAddress": "10.101.235.60",
        "ServerName": "POS048030",
        "StoreNumber": 48,
        "TerminalNumber": 30,
        "Timestamp": "2016-08-12 14:16:25",
        "StatusCode": "PRT_ERR_NOT_FUNCTIONAL",
        "StatusText": "The printer is not functional."
    },


    printerERRClaimed: {
        "MessageType": "Response",
        "Device": "Printer",
        "ResponseType": "Error",
        "ServerIPAddress": "10.101.235.60",
        "ServerName": "POS048030",
        "StoreNumber": 48,
        "TerminalNumber": 30,
        "Timestamp": "2016-08-12 14:16:25",
        "StatusCode": "PRT_ERR_CLAIMED",
        "StatusText": "The printer has already been claimed."
    },
    printerERRClaimedOther: {
        "MessageType": "Response",
        "Device": "Printer",
        "ResponseType": "Error",
        "ServerIPAddress": "10.101.235.60",
        "ServerName": "POS048030",
        "StoreNumber": 48,
        "TerminalNumber": 30,
        "Timestamp": "2016-08-12 14:16:25",
        "StatusCode": "PRT_ERR_CLAIMED_OTHER",
        "StatusText": "The printer has already been claimed by another socket:..."
    },
    printerERRCannotClaim: {
        "MessageType": "Response",
        "Device": "Printer",
        "ResponseType": "Error",
        "ServerIPAddress": "10.101.235.60",
        "ServerName": "POS048030",
        "StoreNumber": 48,
        "TerminalNumber": 30,
        "Timestamp": "2016-08-12 14:16:25",
        "StatusCode": "PRT_ERR_CLAIM",
        "StatusText": "The printer cannot be claimed.  Error:..."
    },
    printerERRNotClaimed: {
        "MessageType": "Response",
        "Device": "Printer",
        "ResponseType": "Error",
        "ServerIPAddress": "10.101.235.60",
        "ServerName": "POS048030",
        "StoreNumber": 48,
        "TerminalNumber": 30,
        "Timestamp": "2016-08-12 14:16:25",
        "StatusCode": "PRT_ERR_NOT_CLAIMED",
        "StatusText": "The printer has not been claimed."
    },
    printerERRPrintNoPaper: {
        "MessageType": "Response",
        "Device": "Printer",
        "ResponseType": "Error",
        "ServerIPAddress": "10.101.235.60",
        "ServerName": "POS048030",
        "StoreNumber": 48,
        "TerminalNumber": 30,
        "Timestamp": "2016-08-12 14:16:25",
        "StatusCode": "PRT_ERR_PRINT",
        "StatusText": "An error occurred while attempting to print.  Error: \"Printer error: (114) (203) Receipt printer is empty\""
    },
    printerERRPrintCoverOpen: {
        "MessageType": "Response",
        "Device": "Printer",
        "ResponseType": "Error",
        "ServerIPAddress": "10.101.235.60",
        "ServerName": "POS048030",
        "StoreNumber": 48,
        "TerminalNumber": 30,
        "Timestamp": "2016-08-12 14:16:25",
        "StatusCode": "PRT_ERR_PRINT",
        "StatusText": "An error occurred while attempting to print.  Error: \"Printer error: (114) (201) Printer cover is open\""
    },
    printerERRPrintFailedToGetOrder: {
        "MessageType": "Response",
        "Device": "Printer",
        "ResponseType": "Error",
        "ServerIPAddress": "10.101.235.60",
        "ServerName": "POS048030",
        "StoreNumber": 48,
        "TerminalNumber": 30,
        "Timestamp": "2016-08-12 14:16:25",
        "StatusCode": "PRT_ERR_PRINT",
        "StatusText": "An error occurred while attempting to print.  Error: \"Error while attempting to retrieve order detail information\""
    },
    printerERRPrintFailed: {
        "MessageType": "Response",
        "Device": "Printer",
        "ResponseType": "Error",
        "ServerIPAddress": "10.101.235.60",
        "ServerName": "POS048030",
        "StoreNumber": 48,
        "TerminalNumber": 30,
        "Timestamp": "2016-08-12 14:16:25",
        "StatusCode": "PRT_ERR_PRINT",
        "StatusText": "An error occurred while attempting to print.  Error: \"Printer error: (111) Failure\""
    }
};

/**
 * Helper function for escaping input strings
 */
function htmlEntities(str) {
    return String(str).replace(/&/g, '&amp;').replace(/</g, '&lt;')
                      .replace(/>/g, '&gt;').replace(/"/g, '&quot;');
}


/**
 * HTTP server
 */
var server = http.createServer(function (request, response) {
    // Not important for us. We're writing WebSocket server, not HTTP server
});
server.listen(webSocketsServerPort, function () {
    console.log((new Date()) + " Server is listening on port " + webSocketsServerPort);
});

/**
 * WebSocket server
 */
var wsServer = new webSocketServer({
    // WebSocket server is tied to a HTTP server. WebSocket request is just
    // an enhanced HTTP request. For more info http://tools.ietf.org/html/rfc6455#page-6
    httpServer: server,
    dropConnectionOnKeepaliveTimeout: false
});
var connection;
var stdin = process.stdin;
var stdout = process.stdout;
stdin.resume();

stdin.on('data', function (data) {
    data = data.toString().trim();

    if (data === "exit") { process.exit(); }
    else if (data === "ls") {
        var arrayOfProperties = Object.keys(responses);
        for (var i = 0; i < arrayOfProperties.length; i++) {
            stdout.write('\n ' + arrayOfProperties[i]);
        }
        stdout.write('\n    ');
    }
    else if (responses[data] !== undefined) {
        var json = JSON.stringify(responses[data]);
        stdout.write('\n\n' + "SENT: " + json + "\n");
        connection.sendUTF(json);
    }
    else {
        var json = JSON.stringify(data);
        stdout.write('\n\n' + "SENT: " + json + "\n\n");
        connection.sendUTF(json);
    }
});

// This callback function is called every time someone
// tries to connect to the WebSocket server
wsServer.on('request', function (request) {

    stdout.write('\n\n' + (new Date()) + ' Connection from origin ' + request.origin + '.');

    // accept connection - you should check 'request.origin' to make sure that
    // client is connecting from your website
    // (http://en.wikipedia.org/wiki/Same_origin_policy)
    connection = request.accept(null, request.origin);
    //wsServer.readyState = 1;
    // we need to know client index to remove them on 'close' event


    // send back chat history
    // if (history.length > 0) {
    //    connection.sendUTF(JSON.stringify( { type: 'history', data: history} ));
    // }

    connection.on('connect', function (connection) {
        connection.readyState = 1;
        stdout.write('\n\n' + 'connection hit connected');
    });
    // user sent some message
    connection.on('message', function (message) {
        if (message.type === 'utf8') { // accept only text
            // log and broadcast the message
            stdout.write('\n\n' + (new Date()) + ' Received Message: ' + message.utf8Data + '\n\n');

            // broadcast message to all connected clients

        } else {
            stdout.write('\n\n' + 'skipped input');
        }
    });

    // user disconnected
    connection.on('close', function (connection) {
        stdout.write('\n\n' + 'connection hit close.');
    });

});