'use strict';

tradingApp.filter('bool', function() {
    return function(input, valueTrue, valueFalse) {
      return input !== true ? valueFalse : valueTrue;
    };
  }
);