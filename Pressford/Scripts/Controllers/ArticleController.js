app.controller("ArticleController", ["$scope", "$http", "articleData", function($scope, $http, articleData) {
    $scope.likes = articleData.likes;
    $scope.comments = articleData.comments;
    $scope.articleId = articleData.articleId;
    if (!articleData.likedByUser) {
        $scope.likeText = 'Like <span class="glyphicon glyphicon-thumbs-up" aria-hidden="true"></span>';
    } else {
        $scope.likeText = 'Unlike <span class="glyphicon glyphicon-thumbs-down" aria-hidden="true"></span>';
    }

    $scope.commentText = "";

    $scope.toggleLike = function () {
        $http({ method: "POST", url: "/Article/ToggleLike?articleId=" + $scope.articleId })
            .success(function (likeToggled) {
                if (likeToggled) {
                    if ($scope.likeText[0] === "L") {
                        $scope.likeText = 'Unlike <span class="glyphicon glyphicon-thumbs-down" aria-hidden="true"></span>';
                        $scope.likes = $scope.likes + 1;
                    } else {
                        $scope.likeText = 'Like <span class="glyphicon glyphicon-thumbs-up" aria-hidden="true"></span>';
                        $scope.likes = $scope.likes - 1;
                    }
                } else {
                    alert("You have liked too many articles today. Please try again tomorrow.");
                }
            });
    };

    $scope.postComment = function() {
        if ($scope.commentText === "") {
            alert("Please write some text in your comment");
            return;
        }
        $http({ method: "POST", url: "/Article/AddComment?articleId=" + $scope.articleId + "&text=" + $scope.commentText })
            .success(function (data) {
                if (data === false) {
                    alert("Your message was not posted. Please try again later.");
                } else {
                    $scope.commentText = "";
                    $scope.comments.push(data);
                }
        });
    };
}]);