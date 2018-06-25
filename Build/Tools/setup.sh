#!/bin/bash
export LC_ALL=en_US.UTF-8
export LANG=en_US.UTF-8
export PATH="/usr/local/bin:$PATH"

xcode-select --install

brew install ruby

gem install bundler
bundle install

brew install librsvg
brew install ImageMagick
brew install gradle
