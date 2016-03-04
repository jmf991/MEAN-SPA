angular.module('appRoutes', []).config(['$routeProvider', '$locationProvider', function ($routeProvider, $locationProvider) {

    $routeProvider

		// home page
		.when('/', {
		    templateUrl: 'components/main/home.html',
		    controller: 'MainController'
		})

		.when('/nerds', {
		    templateUrl: 'components/nerd/nerd.html',
		    controller: 'NerdController'
		})

		.when('/geeks', {
		    templateUrl: 'components/geek/geek.html',
		    controller: 'GeekController'
		})

	    .when('/tronxs', {
	        templateUrl: 'components/tronx/tronx.html',
	        controller: 'TronxController'
	    })

    .when('/tronxsOpcion1', {
        templateUrl: 'components/tronx/TronxOpcion1/TronxOpcion1.html',
        controller: 'TronxOpcion1Controller'
    })

    .when('/tronxsOpcion2', {
        templateUrl: 'components/tronx/TronxOpcion2/tronxOpcion2.html',
        controller: 'tronxOpcion2Controller'
    })
        .when('/tronxsOpcion3', {
            templateUrl: 'components/tronx/TronxOpcion3/tronxOpcion3.html',
            controller: 'tronxOpcion3Controller'
        });

    $locationProvider.html5Mode(true);

}]);