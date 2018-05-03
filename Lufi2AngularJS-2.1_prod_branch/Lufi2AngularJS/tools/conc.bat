echo "Path:%1"
echo "Configuration:%2"

%1tools\jsmin.exe < %1assets/app/app.js   > %1assets\app\services.js
%1tools\jsmin.exe < %1assets/app/appDirectives.js   >> %1assets\app\services.js
%1tools\jsmin.exe < %1assets/app/appServicesCustomer.js   >> %1assets\app\services.js
%1tools\jsmin.exe < %1assets/app/appServicesOrder.js   >> %1assets\app\services.js
%1tools\jsmin.exe < %1assets/app/appServiceOrderCart.js   >> %1assets\app\services.js
%1tools\jsmin.exe < %1assets/app/appServicesItem.js   >> %1assets\app\services.js
%1tools\jsmin.exe < %1assets/app/appServicesPayment.js   >> %1assets\app\services.js
%1tools\jsmin.exe < %1assets/app/appUtilities.js   >> %1assets\app\services.js
%1tools\jsmin.exe < %1assets/app/appservicesWebSocket.js   >> %1assets\app\services.js
%1tools\jsmin.exe < %1assets/app/appservicesAnnouncements.js   >> %1assets\app\services.js
%1tools\jsmin.exe < %1assets/app/appFilters.js   >> %1assets\app\services.js
%1tools\jsmin.exe < %1assets/app/appServicesCommonCode.js   >> %1assets\app\services.js
%1tools\jsmin.exe < %1assets/app/appServicesGiftRegistry.js   >> %1assets\app\services.js
%1tools\jsmin.exe < %1assets/app/appServiceReprice.js   >> %1assets\app\services.js
%1tools\jsmin.exe < %1assets/app/controllers/addModifyAddress.js   >> %1assets\app\services.js
%1tools\jsmin.exe < %1assets/app/appServiceAppState.js   >> %1assets\app\services.js
%1tools\jsmin.exe < %1assets/app/controllers/orderNotesCtrl.js   >> %1assets\app\services.js

%1tools\jsmin.exe < %1assets/app/controllers/couponsCtrl.js   > %1assets\app\controllers.js
%1tools\jsmin.exe < %1assets/app/controllers/couponCtrl.js   >> %1assets\app\controllers.js
%1tools\jsmin.exe < %1assets/app/controllers/indexCtrl.js   >> %1assets\app\controllers.js
%1tools\jsmin.exe < %1assets/app/controllers/homeCtrl.js   >> %1assets\app\controllers.js
%1tools\jsmin.exe < %1assets/app/controllers/swipeCardCtrl.js   >> %1assets\app\controllers.js
%1tools\jsmin.exe < %1assets/app/controllers/itemLocateCtrl.js   >> %1assets\app\controllers.js
%1tools\jsmin.exe < %1assets/app/controllers/customerSearchCtrl.js   >> %1assets\app\controllers.js
%1tools\jsmin.exe < %1assets/app/controllers/customerDetailCtrl.js   >> %1assets\app\controllers.js
%1tools\jsmin.exe < %1assets/app/controllers/modifyCustomerCtrl.js   >> %1assets\app\controllers.js
%1tools\jsmin.exe < %1assets/app/controllers/orderSearchCtrl.js   >> %1assets\app\controllers.js
%1tools\jsmin.exe < %1assets/app/controllers/orderDetailCtrl.js   >> %1assets\app\controllers.js
%1tools\jsmin.exe < %1assets/app/controllers/itemDetailCtrl.js   >> %1assets\app\controllers.js
%1tools\jsmin.exe < %1assets/app/controllers/addModifyAddress.js   >> %1assets\app\controllers.js
%1tools\jsmin.exe < %1assets/app/controllers/itemResultsCtrl.js   >> %1assets\app\controllers.js
%1tools\jsmin.exe < %1assets/app/controllers/itemSearchCtrl.js   >> %1assets\app\controllers.js
%1tools\jsmin.exe < %1assets/app/controllers/orderCartCtrl.js   >> %1assets\app\controllers.js
%1tools\jsmin.exe < %1assets/app/controllers/shippingSelectionCtrl.js   >> %1assets\app\controllers.js
%1tools\jsmin.exe < %1assets/app/controllers/paymentSummaryCtrl.js   >> %1assets\app\controllers.js
%1tools\jsmin.exe < %1assets/app/controllers/returnSummaryCtrl.js   >> %1assets\app\controllers.js