TridionImageSizeChecker
=======================

EventSystem Code to throw a GUI error message if the file being uploaded is too big

Solution Overview
# Capture the Save Event for a Component
# If it is a Multimedia Component and an  'Interesting Mime Type' then continue
# Try to get the Allowed Filesize for the items from the Publication Metadata.  If not found, then use the defaults defined in the MaxFilesizeDef.cs file
# If the file is too big, then throw a new WrongFileSizeException.  For example, "Sorry, file exceeds the allowed size for PDF files.  The allowed size is 10MB."

Special thanks to:

 Dominic Cronin for his Image Size (Height/Width) example here:  https://code.google.com/p/tridion-practice/source/browse/#git%2FImageSizeChecker

 Mihai for the ConfigurationManager class:  https://code.google.com/p/yet-another-tridion-blog/source/browse/trunk/Yet+Another+Event+System/


