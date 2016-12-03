module Profile {
    export class ProfileService {
        private profilesUrlBase: string = 'api/v1/profiles/';

        static $inject = ['$http', 'serviceConfig'];
        constructor(private $http: ng.IHttpService, private serviceConfig) {
        }

        public getProfile(profileId) {
            var url = this.serviceConfig.altidudeApiBaseUri + this.profilesUrlBase + profileId;

            return this.$http.get(url);
        };

        public changeChart(profileId, chartId, base64Image: string): ng.IHttpPromise<{}> {
            var url = this.serviceConfig.altidudeApiBaseUri + this.profilesUrlBase + profileId + '/changechart';

            return this.$http.post(url, { chartId: chartId, base64Image: base64Image });
        };

        public delete(profileId): ng.IHttpPromise<{}> {
            var url = this.serviceConfig.altidudeApiBaseUri + this.profilesUrlBase + profileId + '/delete';

            return this.$http.post(url, {});
        };

        public shareOnFacebook(profile: any): void {
            FB.ui(
                {
                    //method: 'share',
                    //href: 'https://developers.facebook.com/docs/'

                    method: 'share_open_graph',
                    action_type: 'og.likes',
                    action_properties: JSON.stringify({
                    object: 'https://developers.facebook.com/docs/',
                    })

                },
                function (response) {

                    console.log(response);
                    alert('facebook');
                }
            );
        }
    }

    angular.module('altidudeApp').service('profileService', Profile.ProfileService);
}