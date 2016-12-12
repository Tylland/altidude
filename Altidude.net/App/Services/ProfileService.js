var Profile;
(function (Profile) {
    var ProfileService = (function () {
        function ProfileService($http, serviceConfig) {
            this.$http = $http;
            this.serviceConfig = serviceConfig;
            this.profilesUrlBase = 'api/v1/profiles/';
        }
        ProfileService.prototype.getProfile = function (profileId) {
            var url = this.serviceConfig.altidudeApiBaseUri + this.profilesUrlBase + profileId;
            return this.$http.get(url);
        };
        ;
        ProfileService.prototype.changeChart = function (profileId, chartId, base64Image) {
            var url = this.serviceConfig.altidudeApiBaseUri + this.profilesUrlBase + profileId + '/changechart';
            return this.$http.post(url, { chartId: chartId, base64Image: base64Image });
        };
        ;
        ProfileService.prototype.delete = function (profileId) {
            var url = this.serviceConfig.altidudeApiBaseUri + this.profilesUrlBase + profileId + '/delete';
            return this.$http.post(url, {});
        };
        ;
        ProfileService.prototype.shareOnFacebook = function (profile) {
            FB.ui({
                //method: 'share',
                //href: 'https://developers.facebook.com/docs/'
                method: 'share_open_graph',
                action_type: 'og.likes',
                action_properties: JSON.stringify({
                    object: 'https://developers.facebook.com/docs/',
                })
            }, function (response) {
                console.log(response);
                alert('facebook');
            });
        };
        ProfileService.$inject = ['$http', 'serviceConfig'];
        return ProfileService;
    })();
    Profile.ProfileService = ProfileService;
    angular.module('altidudeApp').service('profileService', Profile.ProfileService);
})(Profile || (Profile = {}));
