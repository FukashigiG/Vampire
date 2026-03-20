public interface IDiscribing
{
    string _name { get; }
    string description { get; }

    // 追加説明要素を持たせるためにこれを追加
    IDiscribing ex_Discribing { get; }
}
