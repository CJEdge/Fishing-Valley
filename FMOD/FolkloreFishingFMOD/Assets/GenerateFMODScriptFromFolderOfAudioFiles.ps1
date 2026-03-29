param(
    [Parameter(Mandatory = $true)]
    [string]$RootFolder
)

$root = (Resolve-Path $RootFolder).Path
$rootFolderName = Split-Path $root -Leaf
$scriptsDir = Join-Path $env:LOCALAPPDATA "FMOD Studio\Scripts"

if (-not (Test-Path $scriptsDir)) {
    New-Item -ItemType Directory -Path $scriptsDir -Force | Out-Null
}

function Sanitize-Name {
    param([string]$Text)
    if ($null -eq $Text) { return "" }
    return ($Text -replace "[\'’]", "" -replace "\s+", "")
}

$sanitizedRootFolderName = Sanitize-Name $rootFolderName
$outputFileName = "GENERATE-$sanitizedRootFolderName.js"
$outputPath = Join-Path $scriptsDir $outputFileName

$result = @()

Get-ChildItem -Path $root -Recurse -File -Filter *.wav |
    Sort-Object FullName |
    ForEach-Object {

        $fullPath = $_.FullName
        $relativePath = $fullPath.Substring($root.Length).TrimStart('\','/')
        $relativePath = $relativePath -replace '\\','/'

        $relativeFolder = Split-Path $relativePath -Parent
        if (-not $relativeFolder) {
            $relativeFolder = ""
        }

        $folderSegments = @()
        if ($relativeFolder -ne "") {
            foreach ($seg in ($relativeFolder -split '/')) {
                if ($seg -ne "") {
                    $folderSegments += (Sanitize-Name $seg)
                }
            }
        }

        $sanitizedRelativeFolder = ($folderSegments -join '/')
        $eventName = Sanitize-Name ([System.IO.Path]::GetFileNameWithoutExtension($_.Name))

        $result += [PSCustomObject]@{
            name = $eventName
            fullPath = ($fullPath -replace '\\','/')
            relativeFolder = $sanitizedRelativeFolder
        }
    }

$json = $result | ConvertTo-Json -Depth 10 -Compress
$escapedRoot = $sanitizedRootFolderName.Replace('\','\\').Replace('"','\"')
#HARDCODED, NEEDS CHANGING TO WHATEVER TEMPLATE 
$templatePath = "event:/_Templates/VO_Template"
$escapedTemplatePath = $templatePath.Replace('\','\\').Replace('"','\"')

$js = @"
var FMOD_GENERATED_IMPORT_ROOT_FOLDER = "$escapedRoot";
var FMOD_GENERATED_IMPORT_TEMPLATE_PATH = "$escapedTemplatePath";
var FMOD_GENERATED_IMPORT_DATA = $json;

studio.menu.addMenuItem({
    name: "Generated Import\\$sanitizedRootFolderName",
    execute: function() {

        var ROOT_EVENT_FOLDER_NAME = FMOD_GENERATED_IMPORT_ROOT_FOLDER;
        var TEMPLATE_EVENT_PATH = FMOD_GENERATED_IMPORT_TEMPLATE_PATH;
        //ALSO HARDCODED, NEEDS CHANGING
        var TARGET_BANK_PATH = "bank:/VoiceLines";
        var PARAM_NAME = "ControlScheme";

        var masterEventFolder = studio.project.workspace.masterEventFolder;
        var targetBank = studio.project.lookup(TARGET_BANK_PATH);

        if (!targetBank) {
            alert("Create a bank called VoiceLines first.");
            return;
        }

        function split(path) {
            if (!path) return [];
            return path.replace(/\\/g, "/").split("/").filter(function(p) {
                return p !== "";
            });
        }

        function findOrCreateFolder(parent, name) {
            for (var i = 0; i < parent.items.length; i++) {
                var c = parent.items[i];
                if (c.isOfType("EventFolder") && c.name === name) {
                    return c;
                }
            }

            var f = studio.project.create("EventFolder");
            f.name = name;
            f.folder = parent;
            return f;
        }

        function findEvent(folder, name) {
            for (var i = 0; i < folder.items.length; i++) {
                var c = folder.items[i];
                if (c.isOfType("Event") && c.name === name) {
                    return c;
                }
            }
            return null;
        }

        function findAttachedParameterOnEvent(eventObj, paramName) {
            if (!eventObj.parameters) {
                return null;
            }

            for (var i = 0; i < eventObj.parameters.length; i++) {
                var p = eventObj.parameters[i];

                if (p.preset && p.preset.presetOwner && p.preset.presetOwner.name === paramName) {
                    return p;
                }
            }

            return null;
        }

        function createEventFromTemplate(targetFolder, newName) {
            var template = studio.project.lookup(TEMPLATE_EVENT_PATH);

            if (!template) {
                alert("Could not find template event:\n" + TEMPLATE_EVENT_PATH);
                return null;
            }

            var before = studio.project.model.Event.findInstances();
            var beforeIds = {};
            var i;

            for (i = 0; i < before.length; i++) {
                beforeIds[before[i].id] = true;
            }

            studio.window.navigateTo(template);
            studio.window.triggerAction(studio.window.actions.Duplicate);

            var after = studio.project.model.Event.findInstances();
            var duplicated = null;

            for (i = 0; i < after.length; i++) {
                if (!beforeIds[after[i].id]) {
                    duplicated = after[i];
                    break;
                }
            }

            if (!duplicated) {
                alert("Template duplication failed.");
                return null;
            }

            duplicated.name = newName;
            duplicated.folder = targetFolder;

            return duplicated;
        }

        function importAudio(path) {
            try {
                return studio.project.importAudioFile(path);
            } catch (e) {
                alert("Import failed:\n" + path + "\n\n" + e);
                return null;
            }
        }

        function ensureTrack(eventObj) {
            if (eventObj.groupTracks && eventObj.groupTracks.length > 0) {
                return eventObj.groupTracks[0];
            }
            return eventObj.addGroupTrack();
        }

        function addAudioAcrossParameter(track, param, audioFile) {
            for (var i = 0; i <= 4; i++) {
                var s = track.addSound(param, "SingleSound", i, 1);
                s.audioFile = audioFile;
            }
        }

        function assignBank(eventObj) {
            try {
                eventObj.relationships.banks.add(targetBank);
            } catch (e) {
            }
        }

        var rootFolder = findOrCreateFolder(masterEventFolder, ROOT_EVENT_FOLDER_NAME);

        for (var i = 0; i < FMOD_GENERATED_IMPORT_DATA.length; i++) {
            var item = FMOD_GENERATED_IMPORT_DATA[i];

            var parts = split(item.relativeFolder);
            var currentFolder = rootFolder;

            for (var j = 0; j < parts.length; j++) {
                currentFolder = findOrCreateFolder(currentFolder, parts[j]);
            }

            var eventObj = findEvent(currentFolder, item.name);

            if (!eventObj) {
                eventObj = createEventFromTemplate(currentFolder, item.name);
                if (!eventObj) {
                    continue;
                }
            }

            assignBank(eventObj);

            var eventParam = findAttachedParameterOnEvent(eventObj, PARAM_NAME);
            if (!eventParam) {
                alert("Event '" + eventObj.name + "' does not have attached parameter '" + PARAM_NAME + "'.");
                continue;
            }

            var audio = importAudio(item.fullPath);
            if (!audio) {
                continue;
            }

            var track = ensureTrack(eventObj);
            addAudioAcrossParameter(track, eventParam, audio);
        }

        alert("Done.");
    }
});
"@

Set-Content -Path $outputPath -Value $js -Encoding UTF8

Write-Host ""
Write-Host "Generated FMOD script:"
Write-Host $outputPath
Write-Host ""
Write-Host "Expected template path:"
Write-Host $templatePath
Write-Host ""
Write-Host "Run it inside FMOD:"
Write-Host "Scripts -> Generated Import -> $sanitizedRootFolderName"