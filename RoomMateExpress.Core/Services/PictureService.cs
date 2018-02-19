using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using MvvmCross.Platform;
using MvvmCross.Plugins.Messenger;
using MvvmCross.Plugins.PictureChooser;
using Refit;
using RoomMateExpress.Core.ApiModels;
using RoomMateExpress.Core.Helpers;
using RoomMateExpress.Core.Helpers.Enums;
using RoomMateExpress.Core.Helpers.MvxMessengerHelpers;

namespace RoomMateExpress.Core.Services
{
    public class PictureService : IPictureService
    {
        private readonly IMvxMessenger _messenger;
        private readonly IRoommateExpressApi _api;
        private readonly ILocalizationService _localizationService;

        public PictureService(IMvxMessenger messenger, IRoommateExpressApi api, ILocalizationService localizationService)
        {
            _messenger = messenger;
            _api = api;
            _localizationService = localizationService;
        }

        public async Task RequestPicture(int maxPixelDimension, int percentQuality, PictureOptions options)
        {
            var pictureChooser = Mvx.Resolve<IMvxPictureChooserTask>();
            switch (options)
            {
                case PictureOptions.TakePicture:

                    await ApiRequestHelper.HandlApiRequest(async () =>
                    {
                        string url;
                        using (var stream =
                            await pictureChooser.TakePictureAsync(maxPixelDimension, percentQuality))
                        {
                            if (stream == null)
                            {
                                _messenger.Publish(new PictureMessage(this, string.Empty, Constants.Errors.OperationCanceled, false));
                                return;
                            }
                            url = await _api.UploadPicture(new StreamPart(stream,
                                $"{Guid.NewGuid().ToString()}.jpg", "image/jpeg"));
                        }
                        _messenger.Publish(new PictureMessage(this, url, string.Empty, true));
                    }, error => _messenger.Publish(new PictureMessage(this, string.Empty, error, false)));
                    break;
                case PictureOptions.ChoosePicture:

                    await ApiRequestHelper.HandlApiRequest(async () =>
                    {
                        string url;
                        using (var stream =
                            await pictureChooser.ChoosePictureFromLibrary(maxPixelDimension, percentQuality))
                        {
                            if (stream == null)
                            {
                                _messenger.Publish(new PictureMessage(this, string.Empty, Constants.Errors.OperationCanceled, false));
                                return;
                            }
                            url = await _api.UploadPicture(new StreamPart(stream,
                                $"{Guid.NewGuid().ToString()}.jpg", "image/jpeg"));
                        }
                        _messenger.Publish(new PictureMessage(this, url, string.Empty, true));
                    }, error => _messenger.Publish(new PictureMessage(this, string.Empty, error, false)));
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(options), options, null);
            }
        }

        public async Task<ApiResult> DeletePicture(string filename)
        {
            return await ApiRequestHelper.HandlApiRequest(async () =>
            {
                await _api.DeletePicture(filename);
                return new ApiResult(string.Empty, true);
            });
        }
    }
}
