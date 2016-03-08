angular.module('appRoutes', []).config(['$routeProvider', '$locationProvider', function ($routeProvider, $locationProvider) {

    $routeProvider

		// home page
		.when('/', {
		    templateUrl: 'components/main/authPage.html',
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
        })

        .when('/others', {
            templateUrl: 'components/other/other.html',
            controller: 'otherController'
        })
            .when('/admin', {
                templateUrl: 'components/admin/admin.html',
                controller: 'adminController'
            })
            .when('/archivos', {
                templateUrl: 'components/archivos/archivos.html',
                controller: 'archivosController'
            })
            .when('/ficheros', {
                templateUrl: 'components/ficheros/ficheros.html',
                controller: 'ficherosController'
            })
            .when('/usuarios', {
                templateUrl: 'components/usuarios/usuarios.html',
                controller: 'usuariosController'
            })
            .when('/home', {
                templateUrl: 'components/home/home.html',
                controller: 'homeController'
            });

    $locationProvider.html5Mode(true);

}]);