#!/bin/bash
set -e

function printHelp() {
    echo "Bump version script"
    echo "Select one of: "
    echo "revision   major   minor"
    exit 1
}

function bumpRevision() {
    prepareGit
    bumpVersion 3
    commitGit
}

function bumpMinor() {
    prepareGit
    bumpVersion 1
    commitGit
}

function bumpMajor() {
    prepareGit
    bumpVersion 0
    commitGit
}

function bumpVersion() {
    versionFile='DispelToolsApp/Properties/AssemblyInfo.cs'
    local line=$(grep -n "AssemblyVersion" $versionFile | tail -n 1)
    local version=$(echo $line | awk -F[\"\"] '{print $2}')
    local lineNumber=$(echo $line | cut -d: -f1)
    versionArray=(${version//./ })
    local position="$1"
    echo "Previous version: $version"
    if [ "$position" -lt 0 ] || [ "$position" -gt 3 ]; then
        echo "version position $position is not [0-3]"
    fi

    ((versionArray[position]+=1))
    for ((i=position+1; i<${#versionArray[@]}; i++)); do
        versionArray[i]=0
    done

    local version="${versionArray[0]}.${versionArray[1]}.${versionArray[2]}.${versionArray[3]}"
    tag="v${versionArray[0]}.${versionArray[1]}.${versionArray[3]}"

    echo "Next version $version"
    local newLine=$(echo "[assembly: AssemblyVersion(\"$version\")]")
    echo "New tag: $tag"


    read -p "Is that ok?(Y/n) " -n 1 -r
    echo    # (optional) move to a new line
    case $REPLY in
        y|Y|yes|YES|"")
            echo Replacing version...
        ;;
        *)
            echo Aborted
            exit 0
        ;;
    esac

    
    sed -i "${lineNumber}s/.*/$newLine/" $versionFile
}

function prepareGit() {
    git checkout master
    git pull
}

function commitGit() {
    local releaseBranch="release-${versionArray[0]}-${versionArray[1]}-${versionArray[3]}"
    git checkout -b $releaseBranch
    git add $versionFile
    git commit -m "Create $tag release"
    git checkout master
    git merge $releaseBranch
    git branch -D $releaseBranch
    git push
    git tag -a $tag -m "Create $tag release"
    git push origin $tag
}

[ -z "$1" ] && printHelp

case "$1" in
    'revision')
        bumpRevision
    ;;
    'major')
        bumpMajor
    ;;
    'minor')
        bumpMinor
    ;;
    *)
        printhelp
    ;;
esac

exit 0
