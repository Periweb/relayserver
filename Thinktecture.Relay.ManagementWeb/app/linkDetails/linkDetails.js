(function ($, jQuery) {
    "use strict";

    /**
     * @param $scope
     * @param $state
     * @param $stateParams
     * @param $translate
     * @param $timeout
     * @param {NotificationService} notificationService
     * @param {Link} link
     * @param {AppEvents} appEvents
     * @constructor
     */
    function LinkDetailsController($scope, $state, $stateParams, $translate, $timeout, notificationService, link, appEvents) {
        var linkId = $stateParams.id;

        var data = {
            id: linkId
        };

        $scope.tabs = [
            {
                name: 'info'
            },
            {
                name: 'chart',
                onActivate: function () {
                    $scope.$broadcast(appEvents.reloadChart);
                }
            },
            {
                name: 'logs',
                onActivate: function () {
                    $scope.$broadcast(appEvents.reloadLogs);
                }
            },
            {
                name: 'trace',
                onActivate: function () {
                    $scope.$broadcast(appEvents.reloadTraces);
                }
            }
        ];

        function initialize() {
            if ($stateParams.tab) {
                // Check for a guid first, so we tried to reload a trace result.
                var guidRegEx = /^[0-9a-f]{8}-[0-9a-f]{4}-[1-5][0-9a-f]{3}-[89ab][0-9a-f]{3}-[0-9a-f]{12}$/i;

                if ($stateParams.tab.match(guidRegEx)) {
                    // Just set the trace id to open here, so the directive can take it and open the result
                    return $scope.openTraceId = $stateParams.tab;
                }
            }

            setActiveTab();
        }

        function setActiveTab(tabName) {
            if (tabName && trySetTabActive(tabName)) {
                return true;
            }

            if ($stateParams.tab && trySetTabActive($stateParams.tab)) {
                return true;
            }

             // $scope.tabs.info.active = true;
             $scope.activeTabIndex = 0;
        }

        function trySetTabActive(tabName) {
            angular.forEach($scope.tabs, function (tab, index) {
                if (tab.name === tabName) {
                    $scope.activeTabIndex = index;
                    return true;
                }
            });

            return false;
        }

        function onTabActivated() {
            // Needs to be done in a timeout, so uibootstrap has time for invalidating tab selection
            $timeout(function () {
                var activatedTab = $scope.tabs[$scope.activeTabIndex];
                if (activatedTab) {
                    if (activatedTab.onActivate) {
                        activatedTab.onActivate();
                    }

                    $state.go('.', {
                        id: linkId,
                        tab: activatedTab.name
                    }, {
                        notify: false
                    });
                }
            });
        }

        function closeResultTab(event, tabId) {
            event.preventDefault();
            event.stopPropagation();

            $scope.setActiveTab($scope.tabs.info.name);
            delete $scope.tabs[tabId];

            var arrayIndex = -1;

            $scope.traceResults.forEach(function (item, index) {
                if (item.id === tabId) {
                    arrayIndex = index;
                }
            });

            if (arrayIndex > -1) {
                $scope.traceResults.splice(arrayIndex, 1);
                setActiveTab($scope.tabs.trace.name);
            }
        }

        link.getLink(data)
            .then(function (data) {
                $scope.link = data;
                $scope.translationData = {
                    linkName: data.symbolicName
                };
            }, function () {
                $state.go('links');
                $translate('LINK_DETAILS.NOT_FOUND')
                    .then(function (text) {
                        notificationService.error(text);
                    });
            });

        $scope.invokeOnTabActivate = onTabActivated;

        // TODO: Needs refactoring for communication with linkTraceTab-directive
        $scope.setActiveTab = setActiveTab;
        $scope.onTabActivated = onTabActivated;
        $scope.traceResults = [];
        $scope.closeResultTab = closeResultTab;
        $scope.activeTabIndex = 0;
        
        initialize();
    }

    app.module.controller('linkDetailsController', LinkDetailsController);
})();
