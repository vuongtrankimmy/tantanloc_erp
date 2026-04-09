/**
 * TTL ERP - USB Scanner Listener Module
 * Handles HID (Human Interface Device) barcode/QR scanners simulating keyboard input.
 */

window.attendanceScanner = {
    buffer: "",
    timeout: null,
    dotNetRef: null,

    init: function (dotNetReference) {
        this.dotNetRef = dotNetReference;
        console.log("[Scanner] Listener initialized for USB HID devices.");
        
        // Listen to all keydown events globally
        document.addEventListener('keydown', (e) => this.handleKeyDown(e));
    },

    handleKeyDown: function (e) {
        // Clear buffer if there is a long pause (typing manually vs scanner speed)
        clearTimeout(this.timeout);
        this.timeout = setTimeout(() => {
            this.buffer = "";
        }, 50); // 50ms is standard for high-speed scanners.

        // Scanner usually sends 'Enter' at the end of a scan
        if (e.key === 'Enter') {
            if (this.buffer.length >= 3) {
                console.log("[Scanner] Code detected:", this.buffer);
                this.sendToDotNet(this.buffer);
            }
            this.buffer = "";
            e.preventDefault(); // Prevent submmitting forms if any
            return;
        }

        // Add character to buffer (ignore modifier keys)
        if (e.key.length === 1) {
            this.buffer += e.key;
        }
    },

    sendToDotNet: function (code) {
        if (this.dotNetRef) {
            this.dotNetRef.invokeMethodAsync('HandleScannerInput', code);
        }
    },

    playSuccessSound: function() {
        const audio = new Audio('https://assets.mixkit.co/active_storage/sfx/2568/2568.wav');
        audio.play();
    },

    playErrorSound: function() {
        const audio = new Audio('https://assets.mixkit.co/active_storage/sfx/2571/2571.wav');
        audio.play();
    }
};
