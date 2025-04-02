import os
import shutil

version = "2026"
old_version = "2025"

target = "C:\\Users\\Local Admin\\Documents\\GitHub\\Autodesk.Revit.Sdk.Refs\\lib"
source = os.path.join(target, old_version)
print("source", source)

# Fix the path construction - there was a missing separator
originals = os.path.join("C:\\", "Program Files", "Autodesk", f"Revit {version}")
print("originals path:", originals)

# Create target directory before copying
os.makedirs(os.path.join(target, version), exist_ok=True)

for enumerate_file in os.listdir(source):
    file_path = os.path.join(source, enumerate_file)
    if os.path.isfile(file_path):  # Make sure it's a file
        file_name = enumerate_file  # No need for os.path.basename here
        target_file = os.path.join(target, version, file_name)
        
        # The original path was incorrect - don't add version twice
        original_file = os.path.join(originals, file_name)
        
        if os.path.exists(original_file):
            print(f"Copying: {original_file} -> {target_file}")
            shutil.copy(original_file, target_file)
        else:
            print(f"Original file not found: {original_file}")