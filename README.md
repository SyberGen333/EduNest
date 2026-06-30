# EduNest

> An offline-first academic file organizer for Windows that helps students organize educational materials using a structured container ("Nest") system.

EduNest is designed for students who prefer keeping their academic files organized without relying on cloud services or an internet connection. Instead of manually creating folders every semester, EduNest creates a standardized directory structure and provides an easy workspace for storing lectures, activities, answer sheets, and other educational resources.

Everything remains on your own computer, giving you complete control over your files while providing a cleaner and more consistent way to manage school documents.

---

# Features

## Offline First

- No internet connection required
- No cloud account required
- Works completely with local storage
- Files remain on your own computer

---

## Nest-Based Organization

Instead of placing files randomly across multiple folders, EduNest stores everything inside independent **Nests**.

Each Nest represents a complete academic workspace.

Example:

```
John Doe - BSCS
|
+-- Lectures
|
+-- Activities
|
+-- Answer Sheets
```

Multiple Nests can be created for different users, courses, semesters, or purposes.

---

## Automatic Folder Creation

Creating a new Nest instantly generates a standardized folder structure.

No manual folder creation is necessary.

Benefits:

- Consistent organization
- Faster setup
- Cleaner directory layout
- Less chance of misplaced files

---

## Drag and Drop File Management

Simply drag files or folders into EduNest.

Supported operations include:

- Copy
- Move
- Organize
- Rename
- Delete

No complicated import process is required.

---

## Fast File Search

Locate files without manually browsing folders.

Search supports partial filenames, making it easier to find documents even when the full filename is forgotten.

---

## Tree View Navigation

Browse folders using a familiar hierarchical structure.

Features include:

- Expand/Collapse folders
- Quick navigation
- Folder hierarchy visualization

---

## Data Grid Workspace

Files are displayed in an organized table.

Information includes:

- File name
- File type
- Size
- Date modified

Designed to feel similar to Windows File Explorer while remaining focused on academic materials.

---

## Password Protection

Protect your academic workspace with a local password.

Authentication helps prevent unauthorized access when sharing a computer.

Features include:

- Password creation
- Password login
- Password change
- Password visibility toggle

---

## Windows Hello Recovery

If Windows Hello is available on your device, biometric authentication can assist in password recovery.

Examples:

- Fingerprint
- Face Recognition
- PIN

---

## Automatic ZIP Backup

EduNest can automatically create compressed backups whenever the application closes.

Benefits:

- Smaller backup size
- Easy restoration
- Protection against accidental deletion

---

## Activity Logging

Optionally record important operations performed inside EduNest.

Useful for:

- Troubleshooting
- Tracking changes
- Debugging

---

## Human-Friendly Nest Names

Internally, EduNest stores unique identifiers for every Nest.

Users only see clean, readable names.

Example:

Instead of:

```
John_20260701142031_BSCS
```

You simply see:

```
John Doe - BSCS
```

---

## Local Storage

All files remain on your computer.

Nothing is uploaded.

Nothing is synchronized online.

Nothing leaves your device.

---

# System Requirements

| Component | Requirement |
|-----------|-------------|
| Operating System | Windows 10 or Windows 11 |
| Runtime | .NET Desktop Runtime (matching the application's version) |
| Architecture | x64 |
| RAM | 4 GB minimum (8 GB recommended) |
| Storage | At least 500 MB available |
| Internet | Not required |
| Windows Hello | Optional (for biometric recovery) |

---

# Installation

## Step 1

Download the latest EduNest release.

---

## Step 2

Install the required .NET Desktop Runtime if it is not already installed.

---

## Step 3

Extract the application.

---

## Step 4

Run:

```
EduNest.exe
```

No installer is required if using the portable version.

---

# First-Time Setup

## 1. Launch EduNest

Open the application.

---

## 2. Create a Password

On the first launch, create your local password.

This password protects all future access.

---

## 3. Configure Settings

Optional settings include:

- Automatic ZIP Backup
- Activity Logger

---

## 4. Create Your First Nest

Choose:

- Your name
- Course or category

EduNest will automatically create the folder structure.

---

## 5. Start Organizing

Begin adding:

- Lecture slides
- PDF files
- Activities
- Notes
- Assignments
- Answer sheets
- Images
- Programming projects
- Other educational resources

---

# Typical Workflow

## Creating a New Workspace

1. Open EduNest.
2. Select **Create New Nest**.
3. Enter a display name.
4. Choose a course.
5. Click **Create**.

Your workspace is now ready.

---

## Opening an Existing Nest

1. Open EduNest.
2. Select **Open Nest**.
3. Choose the desired Nest.
4. Begin working.

---

## Organizing Files

You can:

- Drag files into folders.
- Move files between folders.
- Rename items.
- Delete unnecessary files.
- Search instantly.

---

## Closing the Application

If automatic backup is enabled:

1. EduNest compresses the current Nest.
2. A ZIP backup is generated.
3. The application closes safely.

---

# Folder Structure Example

```
My Nest
|
+-- Lectures
|    |
|    +-- Week 1.pdf
|    +-- Week 2.pdf
|
+-- Activities
|    |
|    +-- Laboratory 1.docx
|    +-- Quiz.pdf
|
+-- Answer Sheets
     |
     +-- Activity 1.docx
     +-- Midterm.docx
```

---

# Use Cases

EduNest is suitable for:

- College students
- Senior high school students
- Self-learners
- Review centers
- Programming bootcamps
- Offline classrooms
- Personal knowledge organization

---

# Advantages

- Fully offline
- No subscriptions
- No cloud dependency
- Easy to learn
- Familiar Windows interface
- Organized academic workflow
- Fast file retrieval
- Automatic backups
- Password protection
- Lightweight desktop application

---

# Limitations

Current limitations include:

- Windows only
- No cloud synchronization
- No collaboration features
- No automatic file classification
- No per-file encryption
- Windows Hello depends on compatible hardware

---

# Future Improvements

Potential future enhancements include:

- Automatic file categorization
- Cloud synchronization
- Cross-platform support
- Multiple user profiles
- Custom folder templates
- Additional backup options
- Enhanced search capabilities
- Theme customization

---

# Privacy

EduNest respects user privacy.

- No telemetry
- No advertisements
- No online account
- No cloud storage
- No internet connection required
- All files remain under your control

---

# License

See the project's LICENSE file for licensing information.

---

# Built With

- C#
- .NET
- Windows Forms
- Windows File System APIs

---

# Philosophy

EduNest follows a simple principle:

> **Your academic files belong to you. They should stay organized, accessible, and completely under your control—even without an internet connection.**