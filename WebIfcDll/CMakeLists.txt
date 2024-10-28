cmake_minimum_required(VERSION 3.20)

IF (WIN32)
	add_compile_options("$<$<C_COMPILER_ID:MSVC>:/utf-8>")
	add_compile_options("$<$<CXX_COMPILER_ID:MSVC>:/utf-8>")
ENDIF()

add_library(
    WebIfcDll
)

	#Add the sources for a module
function(addModule moduleName)
	add_subdirectory(${moduleName})
	source_group(TREE ${CMAKE_CURRENT_SOURCE_DIR}/${moduleName} PREFIX ${moduleName} FILES ${ModuleFiles})
endfunction()

	#Add sources at the root
set(SourceFiles
	${CMAKE_CURRENT_SOURCE_DIR}/Api.cpp
)
set(HeaderFiles
	${CMAKE_CURRENT_SOURCE_DIR}/CMakeLists.txt
)
target_sources(WebIfcDll
	PRIVATE
		${SourceFiles}
	PUBLIC
	FILE_SET HEADERS
	BASE_DIRS ${PROJECT_SOURCE_DIR}
	FILES
		${HeaderFiles}
)
set(ModuleFiles
	${SourceFiles}
	${HeaderFiles}
)
source_group(TREE ${CMAKE_CURRENT_SOURCE_DIR} FILES ${ModuleFiles})

add_library(${namespace}WebIfcDll ALIAS WebIfcDll)

IF (WIN32)
	target_compile_definitions(WebIfcDll PRIVATE
        WINDOWS
        _LIB
    )
ENDIF()

target_compile_features(WebIfcDll PUBLIC
	cxx_std_20
)

set_target_properties(WebIfcDll PROPERTIES
    OUTPUT_NAME "${WebIfcDll_lib_name}"
)

target_include_directories(
    WebIfcDll PUBLIC
    $<BUILD_INTERFACE:${PROJECT_SOURCE_DIR}>
    $<INSTALL_INTERFACE:include>
)

# ---- library installation ----
install(
    TARGETS WebIfcDll
    EXPORT "${targets_export_name}"
    FILE_SET HEADERS
    INCLUDES DESTINATION "${include_install_dir}"
)
