#!/usr/bin/python
import sys, os, glob, psycopg2
from pg_musiclibrary import pg_musiclibrary

def is_version(name):
    if name == "info":
        return False
    if name[0] == 'd':
        try:
            rest = int(name[1:])
            return False
        except ValueError:
            return True
    elif name[0:2] == "cd":
        try:
            rest = int(name[2:])
            return False
        except ValueError:
            return True
    else:
        try:
            rest = int(name)
            return False
        except ValueError:
            return True

def has_lossless(path):
    with os.scandir(path) as it0:
        for item in it0:
            if item.is_dir():
                with os.scandir(path + "\\" + item.name) as it1:
                    for subitem in it1:
                        if not subitem.is_dir():
                            if subitem.name.endswith(".flac"):
                                return True
                            
            else:
                if item.name.endswith(".flac"):
                    return True

                
    return False

def get_folderfilepath(path):
    # path is the path of the directory to check for the following files:
    fn = path + "\\" + "folder.png"
    if os.path.exists(fn) and os.path.isfile(fn):
        return fn
    fn = path + "\\" + "folder.jpg"
    if os.path.exists(fn) and os.path.isfile(fn):
        return fn
    fn = path + "\\" + "folder.jpeg"
    if os.path.exists(fn) and os.path.isfile(fn):
        return fn
    return ""

print(sys.version)
print("there are %d arguments" % len(sys.argv))
print("the arguments are: ", str(sys.argv))

if len(sys.argv) == 1:
    print("\n-->a path argument is expected; e.g. 'm:\\'")
    exit(1)
path = sys.argv[1]

pgc = pg_musiclibrary()
pgc.connect()

with os.scandir(path) as it:
    for entry in it:
        if not entry.name.startswith('.') and entry.is_dir():
            print(entry.name)
            idg = pgc.insert("genre", entry.name)
            #print(idg)
            with os.scandir(path + entry.name) as it1:
                for entry1 in it1:
                    if not entry1.name.startswith('.') and entry1.is_dir():
                        print("." + entry1.name)
                        # check for existence of folder.png or the like
                        image_file = get_folderfilepath(path + entry.name + "\\" + entry1.name)
                        if (image_file):
                            print("." + image_file)
                            blob = open(image_file, 'rb').read()
                            idi = pgc.insert("image", psycopg2.Binary(blob))
                        else:
                            idi = -1;   # -1 id denotes no image for this item
                        ida = pgc.insert("artist", entry1.name, idg, idi)
                        #print(ida)
                        with os.scandir(path + entry.name + "\\" + entry1.name) as it2:
                            for entry2 in it2:
                                if not entry2.name.startswith('.') and entry2.is_dir() and entry2.name != "info":
                                    print(".." + entry2.name)
                                    # check for existence of folder.png or the like
                                    image_file = get_folderfilepath(path + entry.name + "\\" + entry1.name + "\\" + entry2.name)
                                    if (image_file):
                                        print(".." + image_file)
                                        blob = open(image_file, 'rb').read()
                                        idi = pgc.insert("image", psycopg2.Binary(blob))
                                    else:
                                        idi = -1;   # -1 id denotes no image for this item
                                    idw = pgc.insert("work", entry2.name, ida, idi)
                                    #print(idw)
                                    workpath = path + entry.name + "\\" + entry1.name + "\\" + entry2.name
                                    with os.scandir(workpath) as it3:
                                        dcount = 0
                                        for entry3 in it3:
                                            if not entry3.name.startswith('.') and entry3.is_dir() and is_version(entry3.name) == True:
                                                dcount = dcount + 1
                                                print("..." + entry3.name)
                                                # check for existence of folder.png or the like
                                                image_file = get_folderfilepath(workpath + "\\" + entry3.name)
                                                if (image_file):
                                                    print("..." + image_file)
                                                    blob = open(image_file, 'rb').read()
                                                    idi = pgc.insert("image", psycopg2.Binary(blob))
                                                else:
                                                    idi = -1;   # -1 id denotes no image for this item                                                    
                                                idwv = pgc.insert("work_version", entry3.name, idw, has_lossless(workpath + "\\" + entry3.name), idi)
                                                #print(idwv)
                                        if dcount == 0:
                                            name = "[default]"
                                            print("..." + name)
                                            # check for existence of folder.png or the like
                                            image_file = get_folderfilepath(workpath)
                                            if (image_file):
                                                print("..." + image_file)
                                                blob = open(image_file, 'rb').read()
                                                idi = pgc.insert("image", psycopg2.Binary(blob))
                                            else:
                                                idi = -1;   # -1 id denotes no image for this item                                                
                                            idwv = pgc.insert("work_version", name, idw, has_lossless(workpath), idi)
                                            #print(idwv)
                        
                        




pgc.close()
