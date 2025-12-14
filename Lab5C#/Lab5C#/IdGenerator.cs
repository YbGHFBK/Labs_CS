public static class IdGenerator
{
    public static int GetNextId<T>(List<T> list) where T : IHasId
    {
        int id = 1;

        while (true)
        {
            bool isValid = true;
            foreach (var item in list)
            {
                if (item.Id == id)
                    isValid = false; break;
            }

            if (isValid) return id;

            id++;
        }

    }
}