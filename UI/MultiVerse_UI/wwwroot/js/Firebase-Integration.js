var UserDeviceToken = null;

const firebaseConfig = {
    apiKey: "AIzaSyCi75w9z4Zj46dFIq4SaG1lmMNUq95i37Q",
    authDomain: "chat-app-33024.firebaseapp.com",
    projectId: "chat-app-33024",
    storageBucket: "chat-app-33024.appspot.com",
    messagingSenderId: "853498967472",
    appId: "1:853498967472:web:9fabd17c7135dc377c5275",
    measurementId: "G-Q6GS8REBSH"
};

firebase.initializeApp(firebaseConfig);

const messaging = firebase.messaging();

messaging.requestPermission().then(() => {
    getRegToken();
}).catch((error) => {
    console.log("Error requesting permission:", error.message);
});

function getRegToken() {
    messaging.getToken().then((currentToken) => {
        if (currentToken) {
            AddOrEditUserDeviceToken(currentToken);
            UserDeviceToken = currentToken;
            console.log(currentToken);
        } else {
            console.log("No registration token available.");
        }
    }).catch((error) => {
        console.log("Error getting token:", error.message);
    });
}
function AddOrEditUserDeviceToken(UserDeviceToken) {
    var ObjJson = new Object();
    ObjJson.UserDeviceToken = UserDeviceToken;
    requiredFields = ['UserDeviceToken'];
    if (!validateRequiredFields(ObjJson, requiredFields)) {
        return;
    }
    $.ajax({
        url: "/Chat/AddOrEdit_UserDeviceToken",
        type: 'POST',
        contentType: 'application/json',
        data: JSON.stringify(UserDeviceToken),
        dataType: "json",
        success: function (response) {
            console.log(response.ReturnText);
            $("#PageLoader").hide();
        },
        failure: function (response) {
            $("#PageLoader").hide();
        },
        error: function (response) {
            $("#PageLoader").hide();
        }
    });
}
function SendNotification(url, Title, Body) {
    var ObjJson = new Object();
    ObjJson.Title = Title;
    ObjJson.Body = Body;
    requiredFields = ['Title', 'Body'];
    if (!validateRequiredFields(ObjJson, requiredFields)) {
        return;
    }
    $.ajax({
        url: url,
        type: 'POST',
        contentType: 'application/json',
        data: JSON.stringify(ObjJson),
        dataType: "json",
        success: function (response) {
            console.log("Notification Send Successfully!");
            $("#PageLoader").hide();
        },
        failure: function (response) {
            $("#PageLoader").hide();
        },
        error: function (response) {
            $("#PageLoader").hide();
        }
    });
}