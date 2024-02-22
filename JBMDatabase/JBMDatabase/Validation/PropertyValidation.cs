namespace JBMDatabase.Validation
{
    public static class PropertyValidation
    {
        public static bool IsValid<TPk>(this TPk pk) where TPk : IComparable
        {
            bool isValid = false;

            if (pk is null)
                return isValid;

            switch (typeof(TPk).Name)
            {
                case "String":
                    if (!string.IsNullOrWhiteSpace(pk.ToString()))
                        isValid = true;
                    break;

                case "Int32":
                    if (int.Parse(pk.ToString()) > 0)
                        isValid = true;
                    break;

                case "Guid":
                    Guid key = new Guid(pk.ToString());
                    if (key != Guid.Empty || key != new Guid())
                        isValid = true;
                    break;

                default:
                    isValid = false;
                    break;
            }
            return isValid;
        }
    }
}
