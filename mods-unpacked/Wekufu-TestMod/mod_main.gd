extends Node

const AUTHORNAME_MODNAME_DIR := "AuthorName-ModName"
const AUTHORNAME_MODNAME_LOG_NAME := "AuthorName-ModName:Main"

var mod_dir_path := ""
var extensions_dir_path := ""
var translations_dir_path := ""

# Before v6.1.0
# func _init(modLoader = ModLoader) -> void:
func _init() -> void:
	mod_dir_path = ModLoaderMod.get_unpacked_dir().path_join(AUTHORNAME_MODNAME_DIR)
	# Add extensions
	install_script_extensions()
	# Add translations
	add_translations()


func install_script_extensions() -> void:
	extensions_dir_path = mod_dir_path.path_join("extensions")
	# extensions_dir_path = mod_dir_path.path_join("extensions") # Godot 4

		# ModLoaderMod.install_script_extension(extensions_dir_path.plus_file(...))

func add_translations() -> void:
	translations_dir_path = mod_dir_path.path_join("translations")
		# ModLoaderMod.add_translation(translations_dir_path.plus_file(...))

func _ready() -> void:
	Globals.AddModItem("tonic_bottle", "res://mods-unpacked/Wekufu-TestMod/tonic_bottle.json")
	ModLoaderLog.info("Ready!", AUTHORNAME_MODNAME_LOG_NAME)
