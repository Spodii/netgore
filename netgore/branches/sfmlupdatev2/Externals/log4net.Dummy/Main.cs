using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using log4net.Appender;
using log4net.Core;
using log4net.ObjectRenderer;
using log4net.Plugin;
using log4net.Repository;
using log4net.Util;

namespace log4net.Core
{
    public struct LoggingEventData
    {
        public string LoggerName;
        public Level Level;
        public string Message;
        public string ThreadName;
        public DateTime TimeStamp;
        public LocationInfo LocationInfo;
        public string UserName;
        public string Identity;
        public string ExceptionString;
        public string Domain;
        public PropertiesDictionary Properties;
    }

    [Flags]
    public enum FixFlags
    {
        [Obsolete("Replaced by composite Properties")]
        Mdc = 0x01,
        Ndc = 0x02,
        Message = 0x04,
        ThreadName = 0x08,
        LocationInfo = 0x10,
        UserName = 0x20,
        Domain = 0x40,
        Identity = 0x80,
        Exception = 0x100,
        Properties = 0x200,
        None = 0x0,
        All = 0xFFFFFFF,
        Partial = Message | ThreadName | Exception | Domain | Properties,
    }

    [Serializable]
    public class LoggingEvent : ISerializable
    {
        public const string HostNameProperty = "log4net:HostName";
        public const string IdentityProperty = "log4net:Identity";
        public const string UserNameProperty = "log4net:UserName";

        public LoggingEvent(Type a, ILoggerRepository b, string c, Level d, object e, Exception f)
        {
        }

        public LoggingEvent(Type a, ILoggerRepository b, LoggingEventData c, FixFlags d)
        {
        }

        public LoggingEvent(Type a, ILoggerRepository b, LoggingEventData c)
        {
        }

        public LoggingEvent(LoggingEventData data) : this(null, null, data)
        {
        }

        protected LoggingEvent(SerializationInfo info, StreamingContext context)
        {
        }

        public string Domain
        {
            get { return string.Empty; }
        }

        public Exception ExceptionObject
        {
            get { return null; }
        }

        public FixFlags Fix
        {
            get { return FixFlags.None; }
            set { }
        }

        public string Identity
        {
            get { return string.Empty; }
        }

        public Level Level
        {
            get { return Level.All; }
        }

        public LocationInfo LocationInformation
        {
            get { return LocationInfo.Instance; }
        }

        public string LoggerName
        {
            get { return string.Empty; }
        }

        public object MessageObject
        {
            get { return null; }
        }

        public PropertiesDictionary Properties
        {
            get { return PropertiesDictionary.Instance; }
        }

        public string RenderedMessage
        {
            get { return string.Empty; }
        }

        public ILoggerRepository Repository
        {
            get { return LoggerRepository.Instance; }
        }

        public static DateTime StartTime
        {
            get { return default(DateTime); }
        }

        public string ThreadName
        {
            get { return string.Empty; }
        }

        public DateTime TimeStamp
        {
            get { return default(DateTime); }
        }

        public string UserName
        {
            get { return string.Empty; }
        }

        [Obsolete("Use Fix property")]
        public void FixVolatileData()
        {
        }

        [Obsolete("Use Fix property")]
        public void FixVolatileData(bool a)
        {
        }

        protected void FixVolatileData(FixFlags a)
        {
        }

        [Obsolete("Use GetExceptionString instead")]
        public string GetExceptionStrRep()
        {
            return GetExceptionString();
        }

        public string GetExceptionString()
        {
            return string.Empty;
        }

        public LoggingEventData GetLoggingEventData()
        {
            return default(LoggingEventData);
        }

        public LoggingEventData GetLoggingEventData(FixFlags a)
        {
            return default(LoggingEventData);
        }

        public PropertiesDictionary GetProperties()
        {
            return PropertiesDictionary.Instance;
        }

        public object LookupProperty(string a)
        {
            return null;
        }

        public void WriteRenderedMessage(TextWriter a)
        {
        }

        #region ISerializable Members

        public virtual void GetObjectData(SerializationInfo a, StreamingContext b)
        {
        }

        #endregion
    }

    public sealed class LevelMap
    {
        static readonly LevelMap _instance;

        static LevelMap()
        {
            _instance = new LevelMap();
        }

        public Level this[string a]
        {
            get { return Level.All; }
        }

        internal static LevelMap Instance
        {
            get { return _instance; }
        }

        public void Add(string a, int b)
        {
        }

        public void Add(string a, int b, string c)
        {
        }

        public void Add(Level a)
        {
        }

        public void Clear()
        {
        }

        public Level LookupWithDefault(Level a)
        {
            return Level.All;
        }
    }

    public interface ILoggerWrapper
    {
        ILogger Logger { get; }
    }

    [Serializable]
    public sealed class Level : IComparable, IEquatable<Level>
    {
        public static readonly Level Alert;
        public static readonly Level All;
        public static readonly Level Critical;
        public static readonly Level Debug;
        public static readonly Level Emergency;
        public static readonly Level Error;
        public static readonly Level Fatal;
        public static readonly Level Fine;
        public static readonly Level Finer;
        public static readonly Level Finest;
        public static readonly Level Info;
        public static readonly Level Notice;
        public static readonly Level Off;
        public static readonly Level Severe;
        public static readonly Level Trace;
        public static readonly Level Verbose;
        public static readonly Level Warn;
        static readonly Level _instance;

        static Level()
        {
            _instance = new Level(0, null);

            Alert = _instance;
            All = _instance;
            Critical = _instance;
            Debug = _instance;
            Emergency = _instance;
            Error = _instance;
            Fatal = _instance;
            Fine = _instance;
            Finer = _instance;
            Finest = _instance;
            Info = _instance;
            Notice = _instance;
            Off = _instance;
            Severe = _instance;
            Trace = _instance;
            Verbose = _instance;
            Warn = _instance;
        }

        public Level(int a, string b, string c)
        {
        }

        public Level(int a, string b)
        {
        }

        public string DisplayName
        {
            get { return string.Empty; }
        }

        public string Name
        {
            get { return string.Empty; }
        }

        public int Value
        {
            get { return 0; }
        }

        public static int Compare(Level l, Level r)
        {
            return 0;
        }

        public override bool Equals(object o)
        {
            return o is Level;
        }

        public override int GetHashCode()
        {
            return 0;
        }

        #region IComparable Members

        public int CompareTo(object r)
        {
            return 0;
        }

        #endregion

        #region IEquatable<Level> Members

        public bool Equals(Level other)
        {
            return !ReferenceEquals(null, other);
        }

        #endregion

        public static bool operator >(Level l, Level r)
        {
            return false;
        }

        public static bool operator <(Level l, Level r)
        {
            return false;
        }

        public static bool operator >=(Level l, Level r)
        {
            return true;
        }

        public static bool operator <=(Level l, Level r)
        {
            return true;
        }

        public static bool operator ==(Level l, Level r)
        {
            return true;
        }

        public static bool operator !=(Level l, Level r)
        {
            return !(l == r);
        }
    }

    public interface ILogger
    {
        string Name { get; }
        ILoggerRepository Repository { get; }

        bool IsEnabledFor(Level a);

        void Log(Type a, Level b, object c, Exception d);

        void Log(LoggingEvent a);
    }

    [Serializable]
    public class LocationInfo
    {
        static readonly LocationInfo _instance;

        static LocationInfo()
        {
            _instance = new LocationInfo(null);
        }

        public LocationInfo(Type a)
        {
        }

        public LocationInfo(string a, string b, string c, string d)
        {
        }

        public string ClassName
        {
            get { return string.Empty; }
        }

        public string FileName
        {
            get { return string.Empty; }
        }

        public string FullInfo
        {
            get { return string.Empty; }
        }

        internal static LocationInfo Instance
        {
            get { return _instance; }
        }

        public string LineNumber
        {
            get { return string.Empty; }
        }

        public string MethodName
        {
            get { return string.Empty; }
        }
    }
}

namespace log4net.Repository
{
    public abstract class LoggerRepositorySkeleton : ILoggerRepository
    {
        static readonly IAppender[] _emptyAppenders = new IAppender[0];
        static readonly ILogger[] _emptyLoggers = new ILogger[0];

        #region ILoggerRepository Members

        public event LoggerRepositoryConfigurationChangedEventHandler ConfigurationChanged
        {
            add { }
            remove { }
        }

        public event LoggerRepositoryConfigurationResetEventHandler ConfigurationReset
        {
            add { }
            remove { }
        }

        public event LoggerRepositoryShutdownEventHandler ShutdownEvent
        {
            add { }
            remove { }
        }

        public bool Configured
        {
            get { return false; }
            set { }
        }

        public LevelMap LevelMap
        {
            get { return LevelMap.Instance; }
        }

        public string Name
        {
            get { return string.Empty; }
            set { }
        }

        public PluginMap PluginMap
        {
            get { return PluginMap.Instance; }
        }

        public PropertiesDictionary Properties
        {
            get { return PropertiesDictionary.Instance; }
        }

        public RendererMap RendererMap
        {
            get { return RendererMap.Instance; }
        }

        public Level Threshold
        {
            get { return Level.All; }
            set { }
        }

        public ILogger Exists(string name)
        {
            return null;
        }

        public IAppender[] GetAppenders()
        {
            return _emptyAppenders;
        }

        public ILogger[] GetCurrentLoggers()
        {
            return _emptyLoggers;
        }

        public ILogger GetLogger(string a)
        {
            return null;
        }

        public void Log(LoggingEvent a)
        {
        }

        public void ResetConfiguration()
        {
        }

        public void Shutdown()
        {
        }

        #endregion
    }

    public delegate void LoggerRepositoryShutdownEventHandler(object sender, EventArgs e);

    public delegate void LoggerRepositoryConfigurationResetEventHandler(object sender, EventArgs e);

    public delegate void LoggerRepositoryConfigurationChangedEventHandler(object sender, EventArgs e);

    public interface ILoggerRepository
    {
        event LoggerRepositoryConfigurationChangedEventHandler ConfigurationChanged;
        event LoggerRepositoryConfigurationResetEventHandler ConfigurationReset;
        event LoggerRepositoryShutdownEventHandler ShutdownEvent;
        bool Configured { get; set; }
        LevelMap LevelMap { get; }
        string Name { get; set; }
        PluginMap PluginMap { get; }
        PropertiesDictionary Properties { get; }
        RendererMap RendererMap { get; }
        Level Threshold { get; set; }

        ILogger Exists(string a);

        IAppender[] GetAppenders();

        ILogger[] GetCurrentLoggers();

        ILogger GetLogger(string a);

        void Log(LoggingEvent a);

        void ResetConfiguration();

        void Shutdown();
    }

    class LoggerRepository : LoggerRepositorySkeleton
    {
        static readonly LoggerRepository _instance;

        static LoggerRepository()
        {
            _instance = new LoggerRepository();
        }

        internal static LoggerRepository Instance
        {
            get { return _instance; }
        }
    }
}

namespace log4net.Plugin
{
    public interface IPlugin
    {
        string Name { get; }

        void Attach(ILoggerRepository repository);

        void Shutdown();
    }

    public sealed class PluginMap
    {
        static readonly PluginMap _instance;

        static PluginMap()
        {
            _instance = new PluginMap(null);
        }

        public PluginMap(ILoggerRepository a)
        {
        }

        public IPlugin this[string a]
        {
            get { return null; }
        }

        internal static PluginMap Instance
        {
            get { return _instance; }
        }

        public void Add(IPlugin a)
        {
        }

        public void Remove(IPlugin a)
        {
        }
    }
}

namespace log4net.ObjectRenderer
{
    public interface IObjectRenderer
    {
        void RenderObject(RendererMap a, object b, TextWriter c);
    }

    public sealed class DefaultRenderer : IObjectRenderer
    {
        static readonly DefaultRenderer _instance;

        static DefaultRenderer()
        {
            _instance = new DefaultRenderer();
        }

        internal static DefaultRenderer Instance
        {
            get { return _instance; }
        }

        #region IObjectRenderer Members

        public void RenderObject(RendererMap a, object b, TextWriter c)
        {
        }

        #endregion
    }

    public class RendererMap
    {
        static readonly RendererMap _instance;

        static RendererMap()
        {
            _instance = new RendererMap();
        }

        public IObjectRenderer DefaultRenderer
        {
            get { return ObjectRenderer.DefaultRenderer.Instance; }
        }

        internal static RendererMap Instance
        {
            get { return _instance; }
        }

        public void Clear()
        {
        }

        public string FindAndRender(object a)
        {
            return string.Empty;
        }

        public void FindAndRender(object a, TextWriter b)
        {
        }

        public IObjectRenderer Get(Object a)
        {
            return ObjectRenderer.DefaultRenderer.Instance;
        }

        public IObjectRenderer Get(Type a)
        {
            return ObjectRenderer.DefaultRenderer.Instance;
        }

        public void Put(Type a, IObjectRenderer b)
        {
        }
    }
}

namespace log4net
{
    class Logger : ILogger
    {
        static readonly Logger _instance;

        static Logger()
        {
            _instance = new Logger();
        }

        internal static Logger Instance
        {
            get { return _instance; }
        }

        #region ILogger Members

        public string Name
        {
            get { return string.Empty; }
        }

        public ILoggerRepository Repository
        {
            get { return LoggerRepository.Instance; }
        }

        public bool IsEnabledFor(Level a)
        {
            return false;
        }

        public void Log(Type a, Level b, object c, Exception d)
        {
        }

        public void Log(LoggingEvent a)
        {
        }

        #endregion
    }

    class Log : ILog
    {
        static readonly Log _instance;
        static readonly Log[] _instances;

        static Log()
        {
            _instance = new Log();
            _instances = new Log[] { _instance };
        }

        internal static Log Instance
        {
            get { return _instance; }
        }

        internal static Log[] Instances
        {
            get { return _instances; }
        }

        #region ILog Members

        public bool IsDebugEnabled
        {
            get { return false; }
        }

        public bool IsErrorEnabled
        {
            get { return false; }
        }

        public bool IsFatalEnabled
        {
            get { return false; }
        }

        public bool IsInfoEnabled
        {
            get { return false; }
        }

        public bool IsWarnEnabled
        {
            get { return false; }
        }

        public ILogger Logger
        {
            get { return null; }
        }

        public void Debug(object a)
        {
        }

        public void Debug(object a, Exception b)
        {
        }

        public void DebugFormat(string a, params object[] b)
        {
        }

        public void DebugFormat(IFormatProvider a, string b, params object[] c)
        {
        }

        public void Error(object a)
        {
        }

        public void Error(object a, Exception b)
        {
        }

        public void ErrorFormat(string a, params object[] b)
        {
        }

        public void ErrorFormat(IFormatProvider a, string b, params object[] c)
        {
        }

        public void Fatal(object a)
        {
        }

        public void Fatal(object a, Exception b)
        {
        }

        public void FatalFormat(string a, params object[] b)
        {
        }

        public void FatalFormat(IFormatProvider a, string b, params object[] c)
        {
        }

        public void Info(object a)
        {
        }

        public void Info(object a, Exception b)
        {
        }

        public void InfoFormat(string a, params object[] b)
        {
        }

        public void InfoFormat(IFormatProvider a, string b, params object[] c)
        {
        }

        public void Warn(object a)
        {
        }

        public void Warn(object a, Exception b)
        {
        }

        public void WarnFormat(string a, params object[] b)
        {
        }

        public void WarnFormat(IFormatProvider a, string b, params object[] c)
        {
        }

        #endregion
    }

    public sealed class LogManager
    {
        LogManager()
        {
        }

        [Obsolete("Use CreateRepository instead of CreateDomain")]
        public static ILoggerRepository CreateDomain(Type repositoryType)
        {
            return LoggerRepository.Instance;
        }

        [Obsolete("Use CreateRepository instead of CreateDomain")]
        public static ILoggerRepository CreateDomain(string repository)
        {
            return LoggerRepository.Instance;
        }

        [Obsolete("Use CreateRepository instead of CreateDomain")]
        public static ILoggerRepository CreateDomain(string repository, Type repositoryType)
        {
            return LoggerRepository.Instance;
        }

        [Obsolete("Use CreateRepository instead of CreateDomain")]
        public static ILoggerRepository CreateDomain(Assembly repositoryAssembly, Type repositoryType)
        {
            return LoggerRepository.Instance;
        }

        public static ILoggerRepository CreateRepository(Type repositoryType)
        {
            return LoggerRepository.Instance;
        }

        public static ILoggerRepository CreateRepository(string repository)
        {
            return LoggerRepository.Instance;
        }

        public static ILoggerRepository CreateRepository(string repository, Type repositoryType)
        {
            return LoggerRepository.Instance;
        }

        public static ILoggerRepository CreateRepository(Assembly repositoryAssembly, Type repositoryType)
        {
            return LoggerRepository.Instance;
        }

        public static ILog Exists(string name)
        {
            return Log.Instance;
        }

        public static ILog Exists(string repository, string name)
        {
            return Log.Instance;
        }

        public static ILog Exists(Assembly repositoryAssembly, string name)
        {
            return Log.Instance;
        }

        public static ILoggerRepository[] GetAllRepositories()
        {
            return new ILoggerRepository[] { LoggerRepository.Instance };
        }

        public static ILog[] GetCurrentLoggers()
        {
            return Log.Instances;
        }

        public static ILog[] GetCurrentLoggers(string repository)
        {
            return Log.Instances;
        }

        public static ILog[] GetCurrentLoggers(Assembly repositoryAssembly)
        {
            return Log.Instances;
        }

        public static ILog GetLogger(string name)
        {
            return Log.Instance;
        }

        public static ILog GetLogger(string repository, string name)
        {
            return Log.Instance;
        }

        public static ILog GetLogger(Assembly repositoryAssembly, string name)
        {
            return Log.Instance;
        }

        public static ILog GetLogger(Type type)
        {
            return Log.Instance;
        }

        public static ILog GetLogger(string repository, Type type)
        {
            return Log.Instance;
        }

        public static ILog GetLogger(Assembly repositoryAssembly, Type type)
        {
            return Log.Instance;
        }

        [Obsolete("Use GetRepository instead of GetLoggerRepository")]
        public static ILoggerRepository GetLoggerRepository()
        {
            return LoggerRepository.Instance;
        }

        [Obsolete("Use GetRepository instead of GetLoggerRepository")]
        public static ILoggerRepository GetLoggerRepository(string repository)
        {
            return LoggerRepository.Instance;
        }

        [Obsolete("Use GetRepository instead of GetLoggerRepository")]
        public static ILoggerRepository GetLoggerRepository(Assembly repositoryAssembly)
        {
            return LoggerRepository.Instance;
        }

        public static ILoggerRepository GetRepository()
        {
            return LoggerRepository.Instance;
        }

        public static ILoggerRepository GetRepository(string repository)
        {
            return LoggerRepository.Instance;
        }

        public static ILoggerRepository GetRepository(Assembly a)
        {
            return LoggerRepository.Instance;
        }

        public static void ResetConfiguration()
        {
        }

        public static void ResetConfiguration(string a)
        {
        }

        public static void ResetConfiguration(Assembly a)
        {
        }

        public static void Shutdown()
        {
        }

        public static void ShutdownRepository()
        {
        }

        public static void ShutdownRepository(string a)
        {
        }

        public static void ShutdownRepository(Assembly a)
        {
        }
    }

    public interface ILog : ILoggerWrapper
    {
        bool IsDebugEnabled { get; }
        bool IsErrorEnabled { get; }
        bool IsFatalEnabled { get; }
        bool IsInfoEnabled { get; }
        bool IsWarnEnabled { get; }

        void Debug(object message);

        void Debug(object message, Exception exception);

        void DebugFormat(string format, params object[] args);

        void DebugFormat(IFormatProvider provider, string format, params object[] args);

        void Error(object message);

        void Error(object message, Exception exception);

        void ErrorFormat(string format, params object[] args);

        void ErrorFormat(IFormatProvider provider, string format, params object[] args);

        void Fatal(object message);

        void Fatal(object message, Exception exception);

        void FatalFormat(string format, params object[] args);

        void FatalFormat(IFormatProvider provider, string format, params object[] args);

        void Info(object message);

        void Info(object message, Exception exception);

        void InfoFormat(string format, params object[] args);

        void InfoFormat(IFormatProvider provider, string format, params object[] args);

        void Warn(object message);

        void Warn(object message, Exception exception);

        void WarnFormat(string format, params object[] args);

        void WarnFormat(IFormatProvider provider, string format, params object[] args);
    }
}

namespace log4net.Appender
{
    public interface IAppender
    {
        string Name { get; set; }

        void Close();

        void DoAppend(LoggingEvent a);
    }
}

namespace log4net.Util
{
    public class PropertiesDictionary
    {
        static readonly PropertiesDictionary _instance;

        static PropertiesDictionary()
        {
            _instance = new PropertiesDictionary();
        }

        internal static PropertiesDictionary Instance
        {
            get { return _instance; }
        }
    }
}