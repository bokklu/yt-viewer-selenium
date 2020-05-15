using System.ComponentModel;

namespace YTViewer.Contracts
{
    public enum UserAgent
    {
        //New
        [Description("Mozilla/5.0 (Windows NT 6.1; WOW64; rv:54.0) Gecko/20100101 Firefox/54.0")]
        One = 1,
        [Description("Mozilla/5.0 (Windows NT 10.0; WOW64; rv:50.0) Gecko/20100101 Firefox/50.0")]
        Two = 2,
        [Description("Mozilla/5.0 (Windows NT 6.1; WOW64; rv:45.0) Gecko/20100101 Firefox/45.0")]
        Three = 3,
        [Description("Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:60.0) Gecko/20100101 Firefox/60.0")]
        Four = 4,
        [Description("Mozilla/5.0 (Windows NT 6.1; WOW64; rv:44.0) Gecko/20100101 Firefox/44.0")]
        Five = 5,
        [Description("Mozilla/5.0 (Windows NT 10.0; WOW64; rv:51.0) Gecko/20100101 Firefox/51.0")]
        Six = 6,
        [Description("Mozilla/5.0 (Windows NT 5.1; rv:52.0) Gecko/20100101 Firefox/52.0")]
        Seven = 7,
        [Description("Mozilla/5.0 (Windows Horst NT 6.1; WOW64; rv:60.0) Gecko/20100101 Firefox/60.0")]
        Eight = 8,
        [Description("Mozilla/5.0 (Macintosh; Intel Mac OS X 10.13; rv:61.0) Gecko/20100101 Firefox/62AF")]
        Nine = 9,
        [Description("Mozilla/5.0 (X11; Linux i686; rv:64.0) Gecko/20100101 Firefox/64.0")]
        Ten = 10,
        [Description("Mozilla/5.0 (Windows NT 6.1; WOW64; rv:64.0) Gecko/20100101 Firefox/64.0")]
        Eleven = 11,
        [Description("Mozilla/5.0 (X11; Linux i586; rv:63.0) Gecko/20100101 Firefox/63.0")]
        Twelve = 12,
        [Description("Mozilla/5.0 (Windows NT 6.2; WOW64; rv:63.0) Gecko/20100101 Firefox/63.0")]
        Thirteen = 13,
        [Description("Mozilla/5.0 (Macintosh; U; Intel Mac OS X 10.10; rv:62.0) Gecko/20100101 Firefox/62.0")]
        Fourteen = 14,
        [Description("Mozilla/5.0 (Macintosh; Intel Mac OS X 10.14; rv:10.0) Gecko/20100101 Firefox/62.0")]
        Fifteen = 15,

        //Old
        [Description("Mozilla/5.0 (Windows NT 6.1; WOW64; rv:40.0) Gecko/20100101 Firefox/40.1")]
        Sixteen = 16,
        [Description("Mozilla/5.0 (Windows NT 5.1; rv:36.0) Gecko/20100101 Firefox/36.0")]
        Seventeen = 17,
        [Description("Mozilla/5.0 (Windows NT 6.1; WOW64; rv:36.0) Gecko/20100101 Firefox/36.0")]
        Eighteen = 18,
        [Description("Mozilla/5.0 (Android 4.4; Mobile; rv:41.0) Gecko/41.0 Firefox/41.0")]
        Nineteen = 19,
        [Description("Mozilla/5.0 (Android; Mobile; rv:40.0) Gecko/40.0 Firefox/40.0")]
        Twenty = 20,

        //Mobile
        [Description("Mozilla/5.0 (iPhone; CPU iPhone OS 12_1 like Mac OS X) AppleWebKit/605.1.15 (KHTML, like Gecko) FxiOS/7.0.4 Mobile/16B91 Safari/605.1.15")]
        TwentyOne = 21,
        [Description("Mozilla/5.0 (iPhone; CPU iPhone OS 8_3 like Mac OS X) AppleWebKit/600.1.4 (KHTML, like Gecko) FxiOS/1.0 Mobile/12F69 Safari/600.1.4")]
        TwentyTwo = 22,
        [Description("Mozilla/5.0 (Android 8.1.0; Mobile; rv:61.0) Gecko/61.0 Firefox/61.0")]
        TwentyThree = 23
    }
}
