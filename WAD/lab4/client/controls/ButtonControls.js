export class ButtonControls {
    constructor(settings) {
        this.workButtons = {
            play: document.querySelector('.play'),
            close: document.querySelector('.close')
        };
        this.canvasButtons = {
            start: document.querySelector('.start'),
            reset: document.querySelector('.reset'),
            stop: document.querySelector('.stop')
        };
        this.canvas = document.querySelector('.anim');
        this.work = document.querySelector('.work');
        document.querySelector('.anim-background').style.backgroundImage = `url(./${settings.texture})`;
        this.events = document.querySelector('.events');
        this.message = document.querySelector('.click-message');
        this.ctx = this.canvas.getContext('2d');
        this.circle = {
            x: settings.circle.x,
            y: settings.circle.y,
            dx: 0,
            dy: 0,
            reload(dx, dy) {
                this.dx = dx;
                this.dy = dy;
            }
        };
        this.redrawInterval = settings.redrawInterval;
    }

    initButtons() {
        this.workButtons.play.addEventListener('click', (event) => this._playHandler());
        this.workButtons.close.addEventListener('click', (event) => this._closeHandler());
        this.canvasButtons.start.addEventListener('click', () => this._startHandler());
        this.canvasButtons.stop.addEventListener('click', () => this._stopHandler());
        this.canvasButtons.reset.addEventListener('click', () => this._resetHandler());
    }

    _clear() {
        this.ctx.clearRect(0, 0, this.canvas.width, this.canvas.height);
    }

    _showClickMessage(message) {
        this.message.innerHTML = `<h3>${message}!</h3>`;
        window.localStorage.setItem((new Date()).toLocaleTimeString(), `Message: ${message}`);
    }

    _startHandler() {
        this._showClickMessage('Play');
        this._setRandomDirection();
        this.intervalId = setInterval(this._draw.bind(this), this.redrawInterval);
        this.canvasButtons.start.classList.toggle('visible');
        this.canvasButtons.stop.classList.toggle('visible');
    }

    _stopHandler() {
        this._showClickMessage('Stop');
        clearInterval(this.intervalId);
        this._clear();
        this.canvasButtons.stop.classList.toggle('visible');
        this.canvasButtons.start.classList.toggle('visible');
        this.circle.x = 10;
        this.circle.y = 10;
    }

    _outOfFieldHandler() {
        this._showClickMessage('Out of field');
        clearInterval(this.intervalId);
        this.canvasButtons.stop.classList.toggle('visible');
        this.canvasButtons.reset.classList.toggle('visible');
    }

    _resetHandler() {
        this._clear();
        this._showClickMessage('Reset');
        this.canvasButtons.reset.classList.toggle('visible');
        this.canvasButtons.start.classList.toggle('visible');
        this.circle.x = 10;
        this.circle.y = 10;
    }

    _playHandler() {
        this.events.classList.toggle('visible-flex');
        this.work.classList.toggle('visible-flex');
        this.workButtons.play.classList.toggle('visible');
        window.localStorage.setItem((new Date()).toLocaleTimeString(), 'Show Work');
        this.workButtons.close.classList.toggle('visible');
        this.canvas.width = this.canvas.offsetWidth;
        this.canvas.height = this.canvas.offsetHeight;
    }

    _closeHandler() {
        this.workButtons.close.classList.toggle('visible');
        this.work.classList.toggle('visible-flex');
        this.events.classList.toggle('visible-flex');
        this.workButtons.play.classList.toggle('visible');
        window.localStorage.setItem((new Date()).toLocaleTimeString(), 'Close Work');
        const keys = Object.keys(localStorage).splice(Object.keys(localStorage).length - 5);
        this.events.innerHTML = '';
        for(const key of keys) {
            if(key !== 'length') {
                this.events.insertAdjacentHTML('beforeend', `<p>${key}: ${localStorage.getItem(key)}</p>`);
            }
        }
    }

    _draw() {
        this.ctx.clearRect(0, 0, this.canvas.width, this.canvas.height);
        this._drawCircle();
        this._onBoundTouched();
        this.circle.x += this.circle.dx;
        this.circle.y += this.circle.dy;
    }

    _drawCircle() {
        this.ctx.beginPath();
        this.ctx.fillStyle = 'yellow';
        this.ctx.arc(this.circle.x, this.circle.y, 10, 0, Math.PI * 2, true);
        this.ctx.fill();
    }

    _onBoundTouched() {
        if(this.circle.y + this.circle.dy > this.canvas.height - 10  || this.circle.y + this.circle.dy < 10) {
            this.circle.dy = -this.circle.dy;
            this._showClickMessage('Bound touched');

        }
        if(this.circle.x + this.circle.dx < 10) {
            this.circle.dx = -this.circle.dx;
            this._showClickMessage('Bound touched');
        }

        if(this.circle.x + this.circle.dx > this.canvas.width + 10  ) {
            this._outOfFieldHandler();
        }


    }

    _setRandomDirection() {
        const [dx, dy] = [1 + Math.floor(Math.random() * 3),  -1*(1 + Math.floor(Math.random() * 3))];
        this.circle.reload(dx, dy);
    }

}