using System;

namespace RoomMateExpress.Core.Helpers.Enums
{
    [Flags]
    public enum PetOptions
    {
        [ResourceKey(Key = "none_pets")]
        None = 0,
        [ResourceKey(Key = "fish")]
        Fish = 1,
        [ResourceKey(Key = "dog")]
        Dog = 2,
        [ResourceKey(Key = "cat")]
        Cat = 4,
        [ResourceKey(Key = "bird")]
        Bird = 8,
        [ResourceKey(Key = "smallAnimals")]
        SmallAnimal = 16,
        [ResourceKey(Key = "reptile")]
        Reptile = 32,
        [ResourceKey(Key = "other")]
        Other = 64
    }
}
