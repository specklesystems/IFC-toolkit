cmake_minimum_required(VERSION 3.21)

# ---- Dependencies ----

include(FetchContent)

FetchContent_Declare(
  spdlog
  GIT_REPOSITORY https://github.com/gabime/spdlog.git
  GIT_TAG        v1.14.1
  FIND_PACKAGE_ARGS NAMES spdlog
)
FetchContent_Declare(
  glm
  GIT_REPOSITORY https://github.com/g-truc/glm.git
  GIT_TAG        1.0.1
  FIND_PACKAGE_ARGS NAMES glm
)
FetchContent_Declare(
  earcut
  GIT_REPOSITORY https://github.com/mapbox/earcut.hpp.git
  GIT_TAG        v2.2.4
  FIND_PACKAGE_ARGS NAMES earcut
)
FetchContent_Declare(
  tinynurbs
  GIT_REPOSITORY https://github.com/QuimMoya/tinynurbs.git
  GIT_TAG		 47115cd9b6e922b27bbc4ab01fdeac2e9ea597a4
  SOURCE_SUBDIR	 ../
  FIND_PACKAGE_ARGS NAMES tinynurbs
)
FetchContent_Declare(
  CDT
  GIT_REPOSITORY https://github.com/artem-ogre/CDT.git
  GIT_TAG		 4d0c9026b8ec846fe544897e7111f8f9080d5f8a
  SOURCE_SUBDIR	 ../
  FIND_PACKAGE_ARGS NAMES CDT
)
FetchContent_Declare(
  fast_float
  GIT_REPOSITORY https://github.com/fastfloat/fast_float.git
  GIT_TAG		 v6.1.6
  SOURCE_SUBDIR	 ../
  FIND_PACKAGE_ARGS NAMES fast_float
)
FetchContent_Declare(
  emscripten
  GIT_REPOSITORY https://github.com/emscripten-core/emscripten.git
  GIT_TAG		 3.1.70
  SOURCE_SUBDIR	 ../
  FIND_PACKAGE_ARGS NAMES emscripten
)

FetchContent_MakeAvailable(spdlog glm earcut tinynurbs CDT fast_float emscripten)

include_directories(
	${FETCHCONTENT_BASE_DIR}/spdlog-src/include
	${FETCHCONTENT_BASE_DIR}/glm-src
	${FETCHCONTENT_BASE_DIR}/earcut-src/include
	${FETCHCONTENT_BASE_DIR}/tinynurbs-src/include
	${FETCHCONTENT_BASE_DIR}/cdt-src/CDT/include
	${FETCHCONTENT_BASE_DIR}/fast_float-src/include
	${FETCHCONTENT_BASE_DIR}/emscripten-src/system/include
)

if (NOT CMAKE_BUILD_TYPE)
  set(CMAKE_BUILD_TYPE Release)
endif ()

project(
    WebIfcDll
    VERSION 0.1.0
    LANGUAGES CXX
)

# ---- Set C++ standard ----
set(CMAKE_CXX_STANDARD 20)
set(CMAKE_CXX_STANDARD_REQUIRED ON)

# ---- Target parameters ----
set(WebIfcDll_lib_name "WebIfcDll")

# ---- Installation parameters ----
set(generated_dir "${CMAKE_CURRENT_BINARY_DIR}/generated")

set(include_install_dir "include")
set(config_install_dir "lib/cmake/${PROJECT_NAME}")

set(project_config "${generated_dir}/${PROJECT_NAME}Config.cmake")
set(version_config "${generated_dir}/${PROJECT_NAME}ConfigVersion.cmake")
set(targets_export_name "${PROJECT_NAME}Targets")
set(namespace "WebIfcDll::")

set(pkgconfig_install_dir "lib/pkgconfig")

set(project_pkgconfig "${generated_dir}/pkgconfig/WebIfcDll.pc")

# ---- Package configuration ----
include(GNUInstallDirs)
set(INCLUDE_INSTALL_DIR "${CMAKE_INSTALL_INCLUDEDIR}/${PROJECT_NAME}"
    CACHE PATH "Location of header files")
set(SYSCONFIG_INSTALL_DIR "${CMAKE_INSTALL_SYSCONFDIR}/${PROJECT_NAME}"
    CACHE PATH "Location of configuration files")

include(CMakePackageConfigHelpers)

configure_package_config_file(
    "${PROJECT_NAME}Config.cmake.in"
    "${project_config}"
    INSTALL_DESTINATION "${CMAKE_INSTALL_LIBDIR}/cmake/${PROJECT_NAME}"
    PATH_VARS INCLUDE_INSTALL_DIR SYSCONFIG_INSTALL_DIR
)

write_basic_package_version_file(
    "${version_config}"
    VERSION ${WebIfcDll_VERSION}
    COMPATIBILITY SameMinorVersion
)

# ---- pkg-config
configure_file(
    WebIfcDll.pc.in
    "${project_pkgconfig}" @ONLY
)

# generate_export_header()

add_subdirectory(engine_web-ifc)

# ---- cmake config installation ----
install(
    FILES "${project_config}" "${version_config}"
    DESTINATION "${config_install_dir}"
)

install(
    EXPORT "${targets_export_name}"
    NAMESPACE "${namespace}"
    DESTINATION "lib/cmake/${PROJECT_NAME}"
)

# ---- pkg-config installation ----
install(
    FILES "${project_pkgconfig}"
    DESTINATION "${pkgconfig_install_dir}"
)

if(WebIfcDll_TESTS)
    add_subdirectory(WebIfcDlltest)
endif()
