CommentsApp.service('CommentsService', ['$http', function ($http) {
    var strUrl = "";
    
   
    this.getCSlInferComments = function (status) {
        var statusobj = {
            Status:status
        };
       // alert('in service' + status);
        if (rootDir != "/") {
            strUrl = rootDir + 'DashBoard/GetCSLInferences';
        }
        else {
            strUrl = '/DashBoard/GetCSLInferences';
        }
        var response = $http({
            method: "post",
            url: strUrl,
            data: JSON.stringify(statusobj),
            dataType: "json"
        });
        return response;

    }

    
    // Add Comments
    this.addCSLInferComment = function (comments) {
        //alert('in service add');
        if (rootDir != "/") {
            strUrl = rootDir + 'DashBoard/AddCSLInferComment';
        }
        else {
            strUrl = '/DashBoard/AddCSLInferComment';
        }
        var response = $http({
            method: "post",
            url: strUrl,
            data: JSON.stringify(comments),
            dataType: "json"
        });
        return response;
    }
    // Update comment
    this.updateCSLInferComment = function (comments) {
        //alert('in updated service');
        if (rootDir != "/") {
            strUrl = rootDir + 'DashBoard/UpdateCSLInferComment';
        }
        else {
            strUrl = '/DashBoard/UpdateCSLInferComment';
        }

        var response = $http({
            method: "post",
            url: strUrl,
            data: JSON.stringify(comments),
            dataType: "json"
        });
        return response;
    }

    //Delete Employee
    this.DeleteCSLInferComment = function (commentId) {
       // alert('in delete service');
        if (rootDir != "/") {
            strUrl = rootDir + 'DashBoard/DeleteCSLInferenceComment';
        }
        else {
            strUrl = '/DashBoard/DeleteCSLInferenceComment';
        }
        var response = $http({
            method: "post",
            url: strUrl,
            params: {
                commentId: JSON.stringify(commentId)
            }
        });
        return response;
    }
    
  
}]);