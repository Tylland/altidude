var Services;
(function (Services) {
    var ModalService = (function () {
        function ModalService($uibModal) {
            this.$uibModal = $uibModal;
            this.modalDefaults = {
                backdrop: true,
                keyboard: true,
                modalFade: true,
                templateUrl: '/app/partials/modal.html'
            };
            this.modalOptions = {
                closeButtonText: 'Close',
                actionButtonText: 'OK',
                headerText: 'Proceed?',
                bodyText: 'Perform this action?'
            };
        }
        ModalService.prototype.showModal = function (customModalDefaults, customModalOptions) {
            if (!customModalDefaults)
                customModalDefaults = {};
            customModalDefaults.backdrop = 'static';
            return this.show(customModalDefaults, customModalOptions);
        };
        ModalService.prototype.show = function (customModalDefaults, customModalOptions) {
            //Create temp objects to work with since we're in a singleton service
            var tempModalDefaults = {};
            var tempModalOptions = {};
            //Map angular-ui modal custom defaults to modal defaults defined in service
            angular.extend(tempModalDefaults, this.modalDefaults, customModalDefaults);
            //Map modal.html $scope custom properties to defaults defined in service
            angular.extend(tempModalOptions, this.modalOptions, customModalOptions);
            if (!tempModalDefaults.controller) {
                tempModalDefaults.controller = function ($scope, $uibModalInstance) {
                    $scope.modalOptions = tempModalOptions;
                    $scope.modalOptions.ok = function (result) {
                        $uibModalInstance.close(result);
                    };
                    $scope.modalOptions.close = function (result) {
                        $uibModalInstance.dismiss('cancel');
                    };
                };
            }
            return this.$uibModal.open(tempModalDefaults).result;
        };
        ModalService.$inject = ['$uibModal'];
        return ModalService;
    })();
    Services.ModalService = ModalService;
    angular.module('altidudeApp').service('modalService', Services.ModalService);
})(Services || (Services = {}));
//# sourceMappingURL=ModalService.js.map