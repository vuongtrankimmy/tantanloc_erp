/**
 * TTL ERP - USB Scanner Listener Module
 * Handles HID (Human Interface Device) barcode/QR scanners simulating keyboard input.
 */

window.attendanceScanner = {
    buffer: "",
    timeout: null,
    focusInterval: null,
    dotNetRef: null,
    _boundHandleKeyDown: null,

    init: function (dotNetReference) {
        this.dotNetRef = dotNetReference;
        console.log("[Scanner] Listener initialized for Kiosk Mode.");
        
        this._boundFocus = () => { if (this.dotNetRef) this.dotNetRef.invokeMethodAsync('UpdateFocusStatus', true); };
        this._boundBlur = () => { if (this.dotNetRef) this.dotNetRef.invokeMethodAsync('UpdateFocusStatus', false); };
        window.addEventListener('focus', this._boundFocus);
        window.addEventListener('blur', this._boundBlur);
        if (this.dotNetRef) {
            this.dotNetRef.invokeMethodAsync('UpdateFocusStatus', document.hasFocus());
        }
        
        // Xóa sạch Listener/Kế thừa từ trang cũ (để chống dội lệnh đăng ký đúp khi qua tab khác)
        if (this._boundHandleKeyDown) {
            document.removeEventListener('keydown', this._boundHandleKeyDown, true);
        }
        this._boundHandleKeyDown = (e) => this.handleKeyDown(e);
        document.addEventListener('keydown', this._boundHandleKeyDown, true);

        // Chống loạn chu trình dính Focus
        if (this.focusInterval) {
            clearInterval(this.focusInterval);
        }
        this.focusInterval = setInterval(() => {
            if (!document.hasFocus()) {
                window.focus();
            }
        }, 1000);

        // Ép buộc cướp Focus ngay tắp lự khi Màn hình Attendance vừa được chuyển đến!
        window.focus();
    },

    dispose: function () {
        console.log("[Scanner] Tearing down listener (Page Navigation).");
        if (this._boundHandleKeyDown) {
            document.removeEventListener('keydown', this._boundHandleKeyDown, true);
            this._boundHandleKeyDown = null;
        }
        if (this.focusInterval) {
            clearInterval(this.focusInterval);
            this.focusInterval = null;
        }
        if (this._boundFocus) {
            window.removeEventListener('focus', this._boundFocus);
            this._boundFocus = null;
        }
        if (this._boundBlur) {
            window.removeEventListener('blur', this._boundBlur);
            this._boundBlur = null;
        }
        this.dotNetRef = null;
    },

    handleKeyDown: function (e) {
        clearTimeout(this.timeout);
        this.timeout = setTimeout(() => {
            this.buffer = "";
        }, 100); // 100ms: Tăng thời gian chờ lên để an toàn hơn với máy quét Không dây (Wireless) hoặc máy đời cũ.

        // Nếu Buffer đang có dữ liệu (nghĩa là Máy Quét Đang Bắn Mã với tốc độ < 100ms/phím)
        // Lập tức CHẶN các ký tự này tràn ra các Ô Nhập Liệu (TextBox) trên màn hình!
        if (this.buffer.length > 0 && e.key.length === 1) {
            e.preventDefault();
        }

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
