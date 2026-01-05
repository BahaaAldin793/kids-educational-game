# 🎮 Kids Educational Game (Unity & Firebase)

A comprehensive educational gaming platform for children built with **Unity**. The project features a robust cloud-integrated backend using **Firebase** for user management and profile synchronization, alongside three engaging mini-games.

---

## 🔐 Authentication & Profile Management
The game implements a secure and seamless user flow:
* **Firebase Auth:** Integrated Login system to verify existing users.
* **Smart Registration:** New users are guided to a registration flow where they can set their nickname.
* **Avatar Selection:** A custom system allowing children to pick their favorite avatar, which is then merged and saved to their cloud profile.
* **Cloud Sync:** User progress and profile data are persisted using Firebase Realtime Database/Firestore.

---

## 🕹️ Mini-Games Included

The application features three distinct educational challenges:

### 1. 🧩 Puzzle Game
Focuses on spatial awareness and logic. Children must drag and drop pieces to complete a hidden image.

### 2. 🧠 Memory Game
A classic card-matching game designed to enhance short-term memory and visual recognition.

### 3. 🔍 Find It Game (Visual Identification)
A specialized logic game where the player is presented with a **Target Animal Name** and **3 different animal images**. The goal is to correctly identify and click the image that matches the text.

---

## 🛠️ Technical Stack
* **Engine:** Unity (C#)
* **Backend:** Firebase Authentication
* **Database:** Firebase Realtime Database
* **UI/UX:** Optimized for child-friendly interaction with vibrant assets.

---

## 🚀 Getting Started

1.  **Clone the Repository:**
    ```bash
    git clone [https://github.com/BahaaAldin793/kids-educational-game.git](https://github.com/BahaaAldin793/kids-educational-game.git)
    ```
2.  **Firebase Configuration:**
    * Place your `google-services.json` (Android) or `GoogleService-Info.plist` (iOS) in the `Assets/` folder.
    * Enable Email/Password Authentication in your Firebase Console.
3.  **Unity Version:**
    * Recommended to open with **Unity 2021.3 LTS** or newer.
4.  **Play:**
    * Open `Scenes/LoginScene.unity` and press **Play**.

---

# 👤 Author
### **Bahaa Aldin**
### **Khaled Mohamed**
### **Khaled Hamada**

---
*Created for educational impact with Unity & Firebase.*
