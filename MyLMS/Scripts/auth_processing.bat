set arg1=%1
echo %arg1%
set arg2=%2
echo %arg2%
cd %arg2%
echo "Creating auth file"
echo %arg1%> auth.pem
if not exist "./2WayLive" (
    
    echo "Removing old zip"
    del "./2WayLive.zip"
    echo "downloading 2WayLive package..."
    wget https://sanats.s3-us-west-2.amazonaws.com/2WayLive.zip
    unzip 2WayLive.zip
)

rem Need to have method to check if S3 2WayLive zip has changed

copy ".\auth.pem" ".\2WayLive\Center Login"
move ".\auth.pem" ".\2WayLive\2WayCall"
echo "zipping again"
zip -r 2WayLive.zip "./2WayLive"

rem remove auth files so we can reuse it
del ".\2WayLive\Center Login\auth.pem"
del ".\2WayLive\2WayCall\auth.pem"
exit
