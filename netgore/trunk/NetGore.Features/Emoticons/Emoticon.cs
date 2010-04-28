using System.Linq;

namespace NetGore.Features.Emoticons
{
    /// <summary>
    /// The different emoticons that can be used.
    /// </summary>
    public enum Emoticon : byte
    {
        [EmoticonInfo(248)]
        Ellipsis,

        [EmoticonInfo(249)]
        Exclamation,

        [EmoticonInfo(250)]
        Heartbroken,

        [EmoticonInfo(251)]
        Hearts,

        [EmoticonInfo(252)]
        Meat,

        [EmoticonInfo(253)]
        Question,

        [EmoticonInfo(254)]
        Sweat
    }
}