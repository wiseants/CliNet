namespace CliNet
{
    public class Bootstrapper
    {
        #region Fileds

        private static Bootstrapper instance = null;

        #endregion

        #region Constructors

        private Bootstrapper()
        {
            BuildContainer();
        }

        #endregion

        #region Properties

        private static Bootstrapper Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new Bootstrapper();
                }

                return instance;
            }
        }

        #endregion

        #region Private methods

        private void BuildContainer()
        {

        }

        #endregion
    }
}
