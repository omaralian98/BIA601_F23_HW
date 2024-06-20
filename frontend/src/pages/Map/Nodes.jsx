import React, { useEffect, useRef, useState } from "react";
import * as d3 from "d3";
import useFormData from "../../store";

const Maps = () => {
  const { response, mode } = useFormData();
  const multipleMaps =
    mode.link === "mode1" || mode.link === "mode2"
      ? [response.bestRoute]
      : response.trucks;

  console.log(multipleMaps);

  return (
    <div className="flex flex-wrap gap-5 w-screen h-screen p-4">
      {multipleMaps?.map((map, index) => (
        <Graph key={index} map={map?.route || map} />
      ))}
    </div>
  );
};

export default Maps;

export const Graph = ({ map }) => {
  const [windowWidth, setWindowWidth] = useState(window.innerWidth);
  const [windowHeight, setWindowHeight] = useState(window.innerHeight);

  useEffect(() => {
    const handleResize = () => {
      setWindowWidth(window.innerWidth);
      setWindowHeight(windowHeight);
    };

    window.addEventListener("resize", handleResize);
    return () => {
      window.removeEventListener("resize", handleResize);
    };
  }, [windowWidth, windowHeight]);

  const { formDataStorage } = useFormData();
  const [nodes, setNodes] = useState([]);
  const [links, setLinks] = useState([]);
  const svgRef = useRef(null);
  let locations = [];
  let temp2 = [];

  const nodesData = formDataStorage?.locations?.locations_name?.map((e) => ({
    id: e,
    label: e,
  }));

  for (let i = 0; i < map?.length; i++) {
    let start;
    let end;
    start = formDataStorage?.locations?.locations_name[parseInt(map[i])];
    end = formDataStorage?.locations?.locations_name[parseInt(map[i + 1])];
    locations.push({ start, end });
  }

  locations.forEach((location) => {
    formDataStorage?.locations?.distances?.forEach((e) => {
      e.forEach((d) => {
        if (
          d.start === location.start &&
          d.end === (location.end ? location.end : location.start)
        ) {
          temp2.push({
            source: d.start,
            target: d.end,
            distance: d.distance,
          });
        }
      });
    });
  });

  useEffect(() => {
    const nodes = nodesData.map((node) => ({ id: node.id, ...node }));
    const links = temp2.map((link) => ({
      source: nodes.find((n) => n.id === link.source),
      target: nodes.find((n) => n.id === link.target),
      distance: link.distance,
    }));

    const simulation = d3
      .forceSimulation(nodes)
      .force(
        "link",
        d3
          .forceLink(links)
          .distance((d) => d.distance)
          .id((d) => d.id)
      )
      .force("charge", d3.forceManyBody().strength(-300))
      .force("center", d3.forceCenter(windowWidth / 2, windowHeight / 2))
      .on("tick", () => {
        setNodes([...nodes]);
        setLinks([...links]);
      });

    return () => {
      simulation.stop();
    };
  }, [nodesData, temp2]);

  return (
    <div className="flex justify-center items-center overflow-auto w-full h-full border-2 border-black bg-gray-100">
      <svg
        ref={svgRef}
        width={windowWidth}
        height={windowHeight}
        className="overflow-auto my-auto">
        <defs>
          <marker
            id="arrowhead"
            markerWidth="10"
            markerHeight="10"
            refX="15"
            refY="3"
            orient="auto"
            markerUnits="strokeWidth">
            <path d="M0,0 L0,6 L9,3 z" fill="black" />
          </marker>
        </defs>
        {links.map((link, index) => (
          <line
            key={index}
            x1={link.source.x}
            y1={link.source.y}
            x2={link.target.x}
            y2={link.target.y}
            stroke="black"
            markerEnd="url(#arrowhead)"
          />
        ))}
        {nodes.map((node, index) => (
          <g key={index}>
            <circle
              cx={node.x}
              cy={node.y}
              r={7}
              fill={`${index === 0 ? "green" : "red"}`}
            />
            <text
              x={node.x}
              y={node.y}
              dy="-1em"
              textAnchor="middle"
              fontSize="8px"
              fontWeight="bold"
              fill="black">
              {node.label}
            </text>
          </g>
        ))}
      </svg>
    </div>
  );
};
