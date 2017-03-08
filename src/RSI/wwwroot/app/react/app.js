'use strict';

var _ReactDOM = ReactDOM;
var render = _ReactDOM.render;

var Example = function Example() {
    return React.createElement(
        'div',
        null,
        React.createElement(
            'h1',
            null,
            'mortacciopodi'
        )
    );
};

render(React.createElement(Example, null), document.getElementById('container'));

