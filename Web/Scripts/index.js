/**
 * A simple plugin for text animation that can be easily used on any page
 * For better experience open in "Full Page View" - https://cdpn.io/janeRivas/debug/JjjYGQN/VJkxxOaZdQek
 * 
 * Thanks for the idea - <vintage.agency>
 * Made with ? by janeRivas <solovyev.a@icloud.com>
 */
class SVG {
    /**
     * @param {HTMLElement} element - HTML element with text to be animated
     * @param {boolean} isStriped - make striped text (play with it ¯\_(?)_/¯)
     * @param {number} rectWidth - rectangle width
     */
    constructor(element, rectWidth = 20, isStriped = false) {
        this.svgStyles = this._getStyles(element);
        this.rectWidth = rectWidth;
        this.isStriped = isStriped;

        this._init(element);
    }

    /**
     * Initialization
     */
    _init(element) {
        const svg = this._createSVG();
        const text = this._createText();
        const { group, rects } = this._createRects();
        const mask = this._createMask();

        svg.appendChild(text);
        svg.appendChild(group);
        svg.appendChild(mask);
        element.insertAdjacentElement('afterend', svg);

        this._hideElement(element);

        this._initAnimation(text, rects);
    }

    /**
     * Starts the animation
     * @param {function} callback function
     */
    animate(fn) {
        this.animation.eventCallback('onComplete', fn);
        this.animation.play();
    }


    /**
     * Restarts the animation
     * @param {function} callback function
     */
    restart(fn) {
        this.animation.eventCallback('onComplete', fn);
        this.animation.restart();
    }

    /**
     * Gets CSS element properties
     * @param {HTMLElement} element - HTML element
     * @return {object} CSS styles
     */
    _getStyles(element) {
        const styles = window.getComputedStyle(element);

        return {
            text: element.innerText,
            width: styles.width.match(/\d+/)[0],
            height: styles.height.match(/\d+/)[0],
            fontFamily: styles.fontFamily,
            fontSize: styles.fontSize,
            fontWeight: styles.fontWeight,
            textTransform: styles.textTransform,
            color: styles.color,
            letterSpacing: styles.letterSpacing
        };

    }

    /**
     * Creates an SVG element
     * @return {HTMLElement} <svg> element
     */
    _createSVG() {
        const svg = document.createElementNS('http://www.w3.org/2000/svg', 'svg');
        svg.setAttributeNS(null, 'width', this.svgStyles.width);
        svg.setAttributeNS(null, 'height', this.svgStyles.height);
        svg.setAttributeNS(null, 'viewBox', `0 0 ${this.svgStyles.width} ${this.svgStyles.height}`);

        return svg;
    }

    /**
     * Creates an SVG text element
     * @params {boolean} isMask - creates a mask from text
     * @return {HTMLElement} <text> element
     */
    _createText(isMask) {
        const text = document.createElementNS('http://www.w3.org/2000/svg', 'text');
        text.appendChild(document.createTextNode(this.svgStyles.text));
        text.setAttributeNS(null, 'x', '50%');
        text.setAttributeNS(null, 'y', '50%');
        text.setAttributeNS(null, 'font-family', this.svgStyles.fontFamily);
        text.setAttributeNS(null, 'font-size', this.svgStyles.fontSize);
        text.setAttributeNS(null, 'font-weight', this.svgStyles.fontWeight);
        text.setAttributeNS(null, 'letter-spacing', this.svgStyles.letterSpacing);
        if (isMask) {
            text.setAttributeNS(null, 'fill', this.svgStyles.color);
        } else {
            text.setAttributeNS(null, 'fill', 'none');
            text.setAttributeNS(null, 'stroke-dasharray', '1420');
            text.setAttributeNS(null, 'stroke-dashoffset', '1420');
            text.setAttributeNS(null, 'stroke-width', '1');
            text.setAttributeNS(null, 'stroke', this.svgStyles.color);
        }
        text.setAttributeNS(null, 'text-rendering', 'optimizeLegibility');
        text.setAttributeNS(null, 'dominant-baseline', 'middle');
        text.setAttributeNS(null, 'text-anchor', 'middle');

        return text;
    }

    /**
     * Creates an SVG mask element
     * @return {HTMLElement} <mask> element
     */
    _createMask() {
        const defs = document.createElementNS('http://www.w3.org/2000/svg', 'defs');
        const mask = document.createElementNS('http://www.w3.org/2000/svg', 'mask');
        const text = this._createText(true);

        mask.setAttributeNS(null, 'id', 'mask');
        mask.appendChild(text);
        defs.appendChild(mask);

        return defs;
    }

    /**
     * Creates a group of SVG rectangles
     * @return {object} <g> element and list of <rect> elements
     */
    _createRects() {
        const group = document.createElementNS('http://www.w3.org/2000/svg', 'g');
        const numberOfRect = this.svgStyles.width / this.rectWidth + this.svgStyles.height / this.rectWidth / 1.5;
        const rects = [];

        const rectHeight = parseInt(this.svgStyles.height) + this.rectWidth * 3;
        const stripe = this.rectWidth / 2;

        for (let i = 0; i < numberOfRect + 1; i++) {
            if (window.CP.shouldStopExecution(0)) break;
            const rect = document.createElementNS('http://www.w3.org/2000/svg', 'rect');
            rect.setAttributeNS(null, 'x', i * this.rectWidth);
            rect.setAttributeNS(null, 'y', -20);
            /**
             * Hmmm... striped...
             */
            if (this.isStriped) {
                rect.setAttributeNS(null, 'width', stripe);
            } else {
                rect.setAttributeNS(null, 'width', this.rectWidth);
            }
            rect.setAttributeNS(null, 'height', rectHeight);
            rect.setAttributeNS(null, 'fill', this.svgStyles.color);

            rects.push(rect);
            group.appendChild(rect);
        } window.CP.exitedLoop(0);

        group.setAttributeNS(null, 'mask', 'url(#mask)');

        return { group, rects };
    }

    /**
     * Hides an element
     * @param {HTMLElement}
     */
    _hideElement(element) {
        element.style.display = 'none';
    }

    /**
     * Animation initialization
     * @param {HTMLElement} text - <text> element
     * @param {HTMLelement[]} rects - <rect> element
     */
    _initAnimation(text, rects) {
        TweenLite.set(rects, { rotation: 45, scaleX: 0 });

        this.animation = new TimelineLite({ paused: true });
        this.animation.to(text, 4, { strokeDashoffset: 0, ease: Power4.easeInOut });
        this.animation.to(rects, 1, { scaleX: 1, ease: Power1.easeIn }, 1.1);
    }
}



/**
 * Line animation
 */
const tlLines = new TimelineLite();
tlLines.to('#js-lines', 1, { opacity: 1, ease: Power1.easeOut, delay: 1.6 });


/**
 * Init
 */
const svg = new SVG(document.querySelector('[data-svg]'));
svg.animate(() => {
    window.console.log('done');
});


/**
 * Restart animation
 */
const restart = () => {
    svg.restart(() => {
        window.console.log('done');
    });

    tlLines.restart();
};




/**
 * Just in case of slow internet
 */
// const img = new Image()
// img.src = 'https://images.unsplash.com/photo-1544077960-604201fe74bc?ixlib=rb-1.2.1&q=80&fm=jpg&crop=entropy&cs=tinysrgb&w=1920&h=1080&fit=crop&ixid=eyJhcHBfaWQiOjF9'
// img.src = 'https://images.unsplash.com/photo-1525145770691-1eaa1d9a3147?ixlib=rb-1.2.1&q=80&fm=jpg&crop=entropy&cs=tinysrgb&w=1920&h=1080&fit=crop&ixid=eyJhcHBfaWQiOjF9'

// img.onload = function() {
//   document.getElementById('js-page').style.backgroundImage = `url(${this.src}`

//   svg.animate(() => {
//     window.console.log('done')
//   })

//   TweenLite.to('#js-lines', 1, {opacity: 1, ease: Power1.easeOut, delay: 1.6 })
// }