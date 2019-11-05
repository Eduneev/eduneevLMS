set arg1=%1
echo %arg1%
echo "Creating auth file"
echo %arg1% > auth.pem
if exist "./2WayLive.zip" (
    echo "Removing old zip"
    del "./2WayLive.zip"
)

echo "downloading 2WayLive package..."
wget https://sanats.s3-us-west-2.amazonaws.com/2WayLive.zip

unzip 2WayLive.zip
copy ".\auth.pem" ".\2WayLive\Center Login"
move ".\auth.pem" ".\2WayLive\2WayCall"
echo "zipping again"
zip -r 2WayLive.zip "./2WayLive"
rmdir /Q /S 2WayLive

