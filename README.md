# Image Effects Desktop

A cross‑platform desktop app built with **Avalonia UI** and **ImageSharp**.  
This project demonstrates how to apply image effects manually and automatically, and how to use **sync** and **async** programming concepts in .NET.

---

## Features

- **Upload images** (PNG, JPG, JPEG, BMP) using the system file picker.
- **Manual effects**: apply effects one by one with a button click.
- **Auto effects**: run random effects every 3 seconds with start/stop control.
- **Image effects included**:
  - Grayscale
  - Sepia
  - Invert
  - OilPaint
  - Pixelate
  - Vignette
  - Glow
  - Polaroid

---

## Concepts Demonstrated

- **Vertical (system/hardware) calls**  
  Using Avalonia’s `StorageProvider` to access the native file picker and read files directly from the operating system.

- **Horizontal (library) calls**  
  Using ImageSharp to process images in a platform‑independent way (same code works on Windows, macOS, Linux).

- **Sync vs Async**  
  - Sync: applying effects directly on the UI thread for quick operations.  
  - Async: using `await` for file I/O and `Task.Delay` for timed auto effects, keeping the UI responsive.

---

## How to Run

### Requirements
- .NET 7 or .NET 8 SDK
- Works on Windows, macOS, and Linux

### Steps
```bash
git clone https://github.com/your-username/ImageEffectDesktop.git
cd ImageEffectDesktop
dotnet restore
dotnet run
