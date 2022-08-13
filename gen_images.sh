#!/bin/bash

set -euo pipefail

rm -rf out
mkdir -p out
cd orig

for i in {1..10}; do
    for x in `ls`; do
        new_name=`echo $x | sed "s/\.png/_$i.png/"`
        cp $x ../out/$new_name
    done
done
