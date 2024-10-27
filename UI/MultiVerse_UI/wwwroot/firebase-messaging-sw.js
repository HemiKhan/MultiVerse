importScripts('https://www.gstatic.com/firebasejs/6.4.2/firebase-app.js');
importScripts('https://www.gstatic.com/firebasejs/6.4.2/firebase-messaging.js');

const firebaseConfig1 = {
    apiKey: "AIzaSyCi75w9z4Zj46dFIq4SaG1lmMNUq95i37Q",
    authDomain: "chat-app-33024.firebaseapp.com",
    projectId: "chat-app-33024",
    storageBucket: "chat-app-33024.appspot.com",
    messagingSenderId: "853498967472",
    appId: "1:853498967472:web:9fabd17c7135dc377c5275",
    measurementId: "G-Q6GS8REBSH"
};

firebase.initializeApp(firebaseConfig1);

const messaging1 = firebase.messaging();

messaging1.setBackgroundMessageHandler(function (payload) {
    console.log('[Firebase] Background message received:', payload);
    // Customize notification here
    const notificationTitle = 'Background Message Title';
    const notificationOptions = {
        body: 'Background Message body.',
        icon: '/firebase-logo.png'
    };
    return self.registration.showNotification(notificationTitle, notificationOptions);
});
