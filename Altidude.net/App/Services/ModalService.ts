module Services {
    export class ModalService {
        private modalDefaults: any;
        private modalOptions: any;

        static $inject = ['$uibModal'];
        constructor(private $uibModal: any) {
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

        public showModal(customModalDefaults, customModalOptions) : ng.IHttpPromise<{}> {
            if (!customModalDefaults) customModalDefaults = {};
            customModalDefaults.backdrop = 'static';
            return this.show(customModalDefaults, customModalOptions);
        }

        public show(customModalDefaults, customModalOptions) {
            //Create temp objects to work with since we're in a singleton service
            var tempModalDefaults: any = {};
            var tempModalOptions: any = {};

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
                }
            }

            return this.$uibModal.open(tempModalDefaults).result;
        }
    }

    angular.module('altidudeApp').service('modalService', Services.ModalService);
}

