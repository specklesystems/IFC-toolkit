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
	${CMAKE_CURRENT_SOURCE_DIR}/src/cpp/geometry/IfcGeometryLoader.cpp
	${CMAKE_CURRENT_SOURCE_DIR}/src/cpp/geometry/IfcGeometryLoader.h
	${CMAKE_CURRENT_SOURCE_DIR}/src/cpp/geometry/IfcGeometryProcessor.cpp
	${CMAKE_CURRENT_SOURCE_DIR}/src/cpp/geometry/IfcGeometryProcessor.h
	${CMAKE_CURRENT_SOURCE_DIR}/src/cpp/geometry/nurbs.cpp
	${CMAKE_CURRENT_SOURCE_DIR}/src/cpp/geometry/nurbs.h
	${CMAKE_CURRENT_SOURCE_DIR}/src/cpp/geometry/operations/boolean-utils/aabb.h
	${CMAKE_CURRENT_SOURCE_DIR}/src/cpp/geometry/operations/boolean-utils/bvh.h
	${CMAKE_CURRENT_SOURCE_DIR}/src/cpp/geometry/operations/boolean-utils/clip-mesh.h
	${CMAKE_CURRENT_SOURCE_DIR}/src/cpp/geometry/operations/boolean-utils/eps.h
	${CMAKE_CURRENT_SOURCE_DIR}/src/cpp/geometry/operations/boolean-utils/fuzzy-bools.h
	${CMAKE_CURRENT_SOURCE_DIR}/src/cpp/geometry/operations/boolean-utils/geometry.h
	${CMAKE_CURRENT_SOURCE_DIR}/src/cpp/geometry/operations/boolean-utils/intersect-ray-tri.h
	${CMAKE_CURRENT_SOURCE_DIR}/src/cpp/geometry/operations/boolean-utils/is-inside-mesh.h
	${CMAKE_CURRENT_SOURCE_DIR}/src/cpp/geometry/operations/boolean-utils/loop-finder.h
	${CMAKE_CURRENT_SOURCE_DIR}/src/cpp/geometry/operations/boolean-utils/math.h
	${CMAKE_CURRENT_SOURCE_DIR}/src/cpp/geometry/operations/boolean-utils/obj-exporter.h
	${CMAKE_CURRENT_SOURCE_DIR}/src/cpp/geometry/operations/boolean-utils/shared-position.h
	${CMAKE_CURRENT_SOURCE_DIR}/src/cpp/geometry/operations/boolean-utils/svg.h
	${CMAKE_CURRENT_SOURCE_DIR}/src/cpp/geometry/operations/boolean-utils/util.h
	${CMAKE_CURRENT_SOURCE_DIR}/src/cpp/geometry/operations/curve-utils.h
	${CMAKE_CURRENT_SOURCE_DIR}/src/cpp/geometry/operations/geometryutils.h
	${CMAKE_CURRENT_SOURCE_DIR}/src/cpp/geometry/operations/mesh_utils.h
	${CMAKE_CURRENT_SOURCE_DIR}/src/cpp/geometry/representation/geometry.h
	${CMAKE_CURRENT_SOURCE_DIR}/src/cpp/geometry/representation/IfcCurve.cpp
	${CMAKE_CURRENT_SOURCE_DIR}/src/cpp/geometry/representation/IfcCurve.h
	${CMAKE_CURRENT_SOURCE_DIR}/src/cpp/geometry/representation/IfcGeometry.cpp
	${CMAKE_CURRENT_SOURCE_DIR}/src/cpp/geometry/representation/IfcGeometry.h
	${CMAKE_CURRENT_SOURCE_DIR}/src/cpp/modelmanager/ModelManager.cpp
	${CMAKE_CURRENT_SOURCE_DIR}/src/cpp/modelmanager/ModelManager.h
	${CMAKE_CURRENT_SOURCE_DIR}/src/cpp/parsing/IfcFileStream.cpp
	${CMAKE_CURRENT_SOURCE_DIR}/src/cpp/parsing/IfcLoader.cpp
	${CMAKE_CURRENT_SOURCE_DIR}/src/cpp/parsing/IfcLoader.h
	${CMAKE_CURRENT_SOURCE_DIR}/src/cpp/parsing/IfcTokenChunk.cpp
	${CMAKE_CURRENT_SOURCE_DIR}/src/cpp/parsing/IfcTokenStream.cpp
	${CMAKE_CURRENT_SOURCE_DIR}/src/cpp/parsing/IfcTokenStream.h
	${CMAKE_CURRENT_SOURCE_DIR}/src/cpp/parsing/string_parsing.cpp
	${CMAKE_CURRENT_SOURCE_DIR}/src/cpp/schema/ifc-schema.h
	${CMAKE_CURRENT_SOURCE_DIR}/src/cpp/schema/IfcSchemaManager.cpp
	${CMAKE_CURRENT_SOURCE_DIR}/src/cpp/schema/IfcSchemaManager.h
	${CMAKE_CURRENT_SOURCE_DIR}/src/cpp/schema/schema-functions.cpp
	${CMAKE_CURRENT_SOURCE_DIR}/src/cpp/version.h
	${CMAKE_CURRENT_SOURCE_DIR}/src/cpp/web-ifc-test.cpp
	${CMAKE_CURRENT_SOURCE_DIR}/src/cpp/web-ifc-wasm.cpp
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
