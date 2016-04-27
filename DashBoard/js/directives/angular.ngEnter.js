"use strict";

CommentsApp.directive("ngEnter", function () {
	return function  (scope,elem) {
		$(elem).keyup(function  (e) {
			//Enter Keycode is 13
			if (e.keyCode === 13) {
				/*Also update the Angular Cycle*/
				scope.$apply(function  () {
				    scope.addcommnt();//Call addTodo defined inside controller
				});
			}
		});
	};
});