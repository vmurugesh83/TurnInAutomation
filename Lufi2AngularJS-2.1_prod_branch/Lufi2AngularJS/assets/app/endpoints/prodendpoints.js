//Global Endpoint Settings needed for service calls.
var POSURL = "ws://localhost:1510";
var SOLRURL = "http://item.search.prod.bonton.com/solr/item";
var serviceURL = "http://broker.prod.bonton.com:7080";
var tokenizeURL = "https://broker.prod.bonton.com:7083/Tokenizer/Tokenize";
var authURL = "https://broker.prod.bonton.com:7083/Security/UserAuthentication";
var getAnnouncementUrl = "http://broker.prod.bonton.com:7080/Utilities/getExceptionListJSON";
var createAnnouncementUrl = "http://broker.prod.bonton.com:7080/Utilities/createExceptionJSON";
var getNodeListUrl = "http://broker.prod.bonton.com:7080/Utilities/getShipNodeList";
var xAuth = undefined;