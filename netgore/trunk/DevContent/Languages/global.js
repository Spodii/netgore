/* Code in this file will end up in the generated JScript for all languages */

/*
	These constants need to be defined in all languages using the respective text for the language.
	When defining them, remove the trailing underscore, since that is only used for the default for
	the constant (in case the language fails to define it).
	For grabbing these values, please use GetUserConst().
*/
public const _PARAMETER_MISSING = "<Param Missing>";
public const _STR_DAYS = "days";
public const _STR_DAY = "day";
public const _STR_HOURS = "hours";
public const _STR_HOUR = "hour";
public const _STR_MINUTES = "minutes";
public const _STR_MINUTE = "minute";
public const _STR_SECONDS = "seconds";
public const _STR_SECOND = "second";

/*
@summary: Gets the value of a constant, falling back to the default value if it does not exist.
			This is only intended for constants defined by this file (global.js) and is to be used
			so we can fall back to a default value if a constant is not defined.
@params:
	@userconstName: The NAME of the constant, as a string.
@returns: The value of the userConstName, or the default value if it is not defined.
*/
public function GetUserConst(userConstName)
{
	try
	{
		return eval(userConstName);
	}
	catch (e)
	{
		try
		{
			return eval("_" + userConstName);
		}
		catch (e2)
		{
			return "[UNDEFINED CONSTANT: " + userConstName + "]";
		}
	}
}

/*
@summary: Safely gets a parameter, using a predefined string if the parameter is not found.
@params:
	@p: The array containing the parameter to get.
	@i: The index of the item to get.
@returns: The string for the item in the given element, or the value of PARAMETER_MISSING if the element does not exist.
*/
public function GetSafe(p, i)
{
	return p.Length > i ? p[i] : GetUserConst("PARAMETER_MISSING");
}

/*
@summary: Checks if a variable is defined.
@params:
	@v: The variable to check if defined.
@returns: True if the variable is defined; otherwise false.
*/
public function IsDefined(v)
{
	return !(typeof(v) === "undefined");
}

/*
@summary: Gets a nice output string for a timespan.
@params:
	@totalSeconds: The total number of seconds.
@returns: A timespan string using multiple units of time.
*/
public function GetTimeSpanString(totalSeconds) {
	const SECS_PER_DAY = 60*60*24;
	const SECS_PER_HOUR = 60*60;
	const SECS_PER_MIN = 60;
	
	var secsLeft = totalSeconds;
	
	var days = Math.floor(secsLeft / SECS_PER_DAY);
	secsLeft -= days * SECS_PER_DAY;
	
	var hours = Math.floor(secsLeft / SECS_PER_HOUR);
	secsLeft -= hours * SECS_PER_HOUR;
	
	var mins = Math.floor(secsLeft / SECS_PER_MIN);
	secsLeft -= mins * SECS_PER_MIN;
	
	var secs = secsLeft;

	var s = "";
	if (days > 1)
		s += days + " " + GetUserConst("STR_DAYS") + ", ";
	else if (days == 1)
		s += days + " " + GetUserConst("STR_DAY") + ", ";

	if (hours > 1)
		s += hours + " " + GetUserConst("STR_HOURS") + ", ";
	else if (hours == 1)
		s += hours + " " + GetUserConst("STR_HOUR") + ", ";

	if (mins > 1)
		s += mins + " " + GetUserConst("STR_MINUTES") + ", ";
	else if (hours == 1)
		s += mins + " " + GetUserConst("STR_MINUTE") + ", ";

	if (secs > 1)
		s += secs + " " + GetUserConst("STR_SECONDS") + ", ";
	else if (secs == 1)
		s += secs + " " + GetUserConst("STR_SECOND") + ", ";

	if (s.length > 2)
		s = s.substring(0, s.length - 2);

	return s;
}