#!/bin/bash
set -e

function printHelp() {
    echo "Bump version script"
    echo "Select one of: "
    echo "patch   major   minor"
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
    versionFile='View/View.csproj'
    local line=$(grep -n "<Version>" $versionFile | tail -n 1)
    local version=$(echo "$line" | awk -F'[<>]' '{print $3} ')
    local lineNumber=$(echo "$line" | cut -d: -f1)
    local lineNoNumber=$(echo "$line" | cut -d: -f2)
    versionArray=(${version//./ })
    local position="$1"
    echo "$lineNoNumber"
    echo "Previous version: $version"
    if [ "$position" -lt 0 ] || [ "$position" -gt 3 ]; then
        echo "version position $position is not [0-3]"
    fi

    ((versionArray[position]+=1))
    for ((i=position+1; i<${#versionArray[@]}; i++)); do
        versionArray[i]=0
    done

    local newversion="${versionArray[0]}.${versionArray[1]}.${versionArray[2]}.${versionArray[3]}"
    tag="v${versionArray[0]}.${versionArray[1]}.${versionArray[3]}"

    echo "Next version $newversion"
    local escapedVersion=$(echo "$version" | sed 's/[\.]/\\./g')
    local newLine=$(echo "$lineNoNumber" | sed "s/$escapedVersion/$newversion/")
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
    
    local escapedNewLine=$(echo "$newLine" | sed 's/[\/]/\\\//g')
    
    sed -i "${lineNumber}s/.*/$escapedNewLine/" $versionFile
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
    'test')
        bumpVersion 0
        bumpVersion 1
        bumpVersion 3
    ;;
    'patch')
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
