# Distributed under the OSI-approved BSD 3-Clause License.  See accompanying
# file Copyright.txt or https://cmake.org/licensing for details.

cmake_minimum_required(VERSION 3.5)

file(MAKE_DIRECTORY
  "C:/Users/cdigg/git/temp/engine_web-ifc/src/cpp/_deps/fastfloat-src"
  "C:/Users/cdigg/git/temp/engine_web-ifc/src/cpp/_deps/fastfloat-build"
  "C:/Users/cdigg/git/temp/engine_web-ifc/src/cpp/_deps/fastfloat-subbuild/fastfloat-populate-prefix"
  "C:/Users/cdigg/git/temp/engine_web-ifc/src/cpp/_deps/fastfloat-subbuild/fastfloat-populate-prefix/tmp"
  "C:/Users/cdigg/git/temp/engine_web-ifc/src/cpp/_deps/fastfloat-subbuild/fastfloat-populate-prefix/src/fastfloat-populate-stamp"
  "C:/Users/cdigg/git/temp/engine_web-ifc/src/cpp/_deps/fastfloat-subbuild/fastfloat-populate-prefix/src"
  "C:/Users/cdigg/git/temp/engine_web-ifc/src/cpp/_deps/fastfloat-subbuild/fastfloat-populate-prefix/src/fastfloat-populate-stamp"
)

set(configSubDirs Debug)
foreach(subDir IN LISTS configSubDirs)
    file(MAKE_DIRECTORY "C:/Users/cdigg/git/temp/engine_web-ifc/src/cpp/_deps/fastfloat-subbuild/fastfloat-populate-prefix/src/fastfloat-populate-stamp/${subDir}")
endforeach()
if(cfgdir)
  file(MAKE_DIRECTORY "C:/Users/cdigg/git/temp/engine_web-ifc/src/cpp/_deps/fastfloat-subbuild/fastfloat-populate-prefix/src/fastfloat-populate-stamp${cfgdir}") # cfgdir has leading slash
endif()