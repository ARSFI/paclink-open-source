using NLog.Config;
using NLog.Layouts;


namespace NLog.Targets.Gelf
{
    [NLogConfigurationItem]
    public class GelfParameterInfo
    {
         public GelfParameterInfo()
            : this(null, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DatabaseParameterInfo" /> class.
        /// </summary>
        /// <param name="parameterName">Name of the parameter.</param>
        /// <param name="parameterLayout">The parameter layout.</param>
         public GelfParameterInfo(string parameterName, Layout parameterLayout)
        {
            this.Name = parameterName;
            this.Layout = parameterLayout;
        }

         /// <summary>
         /// Gets or sets the database parameter name.
         /// </summary>
         /// <docgen category='Parameter Options' order='10' />
         [RequiredParameter]
         public string Name { get; set; }

         /// <summary>
         /// Gets or sets the layout that should be use to calcuate the value for the parameter.
         /// </summary>
         /// <docgen category='Parameter Options' order='10' />
         [RequiredParameter]
         public Layout Layout { get; set; }

    }
}
