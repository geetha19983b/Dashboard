var CommentsApp = angular.module('CommentsApp', [])

CommentsApp.controller('CommentsController', function ($scope, CommentsService) {

    

    $scope.init = function (status) {

        //if (localStorageService.get("todoList") === null) {
        //    getCSlInferComments();
        //} else {
        //    $scope.model = localStorageService.get("commntList");
        //}
       // alert('in init' + status);
        getCSlInferComments(status);

        $scope.display = status;
        //$scope.model = $scope.cslcomments;
        $scope.show = "All";
        $scope.currentShow = 0;
    }




    function getCSlInferComments(status) {
        CommentsService.getCSlInferComments(status)
        .success(function (cslcomment) {

            // $scope.cslcomments = cslcomment;

            $scope.model = cslcomment;

            var categories = $scope.model[0].Categories;
            var catpresent = [];

            angular.forEach($scope.model, function (item) {

                catpresent.push(item.Category);

            });

            //alert(catpresent);
            angular.forEach(categories, function (value, key) {
                if (catpresent.indexOf(value) < 0) {
                    $scope.model.push({ Category: value, CLSList: [] })
                }

            });


            //alert(categories);
            console.log($scope.model);

        })

            .error(function (error) {

                $scope.status = 'Unable to load comments data: ' + error.message;

                console.log($scope.status);

            });
    }



    $scope.addcommnt = function (apprvstatus) {
        //alert('in add' + $scope.newcommnt + $scope.currentShow);
        /*Should prepend to array*/
        //$scope.model[$scope.currentShow].CLSList.splice(0, 0, { Comments: $scope.newcommnt, Approved: false });
        //alert('in add' + $scope.model[$scope.currentShow].Category + $scope.newcommnt);

        //var Category = scope.model[$scope.currentShow].Category;
        //var Comment =  $scope.newcommnt;
        var status = apprvstatus;
        //alert('status passed ' + apprvstatus + $scope.display);
        var comments = {
            Category: $scope.model[$scope.currentShow].Category,
            Comment: $scope.newcommnt,
            Approved: status
        };

        //var getData = CommentsService.updateCSLInferComment(commnt);
        var getData = CommentsService.addCSLInferComment(comments);
        getData.then(function (msg) {
            
            getCSlInferComments($scope.display);
            alert(msg.data);
            //alert('Comments added');


        }, function () {
            alert('Error in adding record');
        });

        /*Reset the Field*/
        $scope.newcommnt = "";
    };
    //$scope.addcommnt1 = function (approvstatus) {
    //    $scope.Language = approvstatus;
    //};
    $scope.deletecommnt = function (index) {
        //$scope.model[$scope.currentShow].CLSList.splice(index, 1);
        //alert(index.CommentID);

        var getData = CommentsService.DeleteCSLInferComment(index.CommentID);
        getData.then(function (msg) {
            getCSlInferComments($scope.display);
            alert('Comment Deleted');
        }, function () {
            alert('Error in Deleting Record');
        });
    };

    $scope.commntSortable = {
        containment: "parent",//Dont let the user drag outside the parent
        cursor: "move",//Change the cursor icon on drag
        tolerance: "pointer"//Read http://api.jqueryui.com/sortable/#option-tolerance
    };

    $scope.changecommnt = function (i) {
        $scope.currentShow = i;
    };

    /* Filter Function for All | Incomplete | Complete */
    $scope.showFn = function (commnt) {

        if ($scope.show === "All") {
            return true;
        } else if (commnt.Approved && $scope.show === "Approved") {
            return true;
        } else if (!commnt.Approved && $scope.show === "NotApproved") {
            return true;
        } else {
            return false;
        }
    };
    $scope.editcomment = function (commnt) {
       // alert('edit comment invoked' + commnt.CommentID + commnt.Comments + commnt.Approved);

        var getData = CommentsService.updateCSLInferComment(commnt);
        getData.then(function (msg) {

            alert(msg.data);

        }, function () {
            alert('Error in updating record');
        });
    }
    //$scope.$watch("model", function (newVal, oldVal) {
    //    if (newVal !== null && angular.isDefined(newVal) && newVal !== oldVal) {
    //        localStorageService.add("commntList", angular.toJson(newVal));
    //    }
    //}, true);


});