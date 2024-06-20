import React, { useEffect, useRef, useState } from "react";
import * as d3 from "d3";
import useFormData from "../../store";

const Maps = () => {
  const { response, mode } = useFormData();
  const multipleMaps =
    mode.link === "mode1" || mode.link === "mode2"
      ? [response.bestRoute]
      : response.trucks;

  return (
    <div className="flex gap-5 flex-wrap w-[100vw] h-[100vh] justify-center">
      {multipleMaps?.length > 1
        ? multipleMaps?.map((map) => <Graph map={map?.route} />)
        : multipleMaps?.map((map) => <Graph map={map} />)}
    </div>
  );
};

export default Maps;

export const Graph = ({ map }) => {
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

  for (let i = 0; i < map.length; i++) {
    let start;
    let end;
    start = formDataStorage?.locations?.locations_name[parseInt(map[i])];
    end = formDataStorage?.locations?.locations_name[parseInt(map[i + 1])];
    locations.push({ start, end });
  }
  locations.map((location, i) => {
    let x = {};
    formDataStorage?.locations?.distances?.map((e) =>
      e.map((d) => {
        if (
          d.start === location.start &&
          d.end === (location.end ? location.end : location.start)
        ) {
          x = {
            source: d.start,
            target: d.end,
            distance: d.distance,
          };
          temp2.push(x);
        }
      })
    );
  });
  useEffect(() => {
    const nodes = nodesData.map((node, index) => ({ id: index, ...node }));
    const links = temp2.map((link) => ({
      source: link.source,
      target: link.target,
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
      .force("center", d3.forceCenter(300, 300)) // Center the simulation
      .on("tick", () => {
        setNodes([...nodes]);
        setLinks([...links]);
      });

    return () => {
      simulation.stop();
    };
  }, [locations, map]);

  return (
    <div className="flex justify-center items-center overflow-auto">
      <svg ref={svgRef} width={600} height={600} className="overflow-auto">
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
        {nodes.length > 0 &&
          nodes.map((node, index) => (
            <g key={index}>
              <circle cx={node.x} cy={node.y} r={7} fill="red" />
              <text
                x={node.x}
                y={node.y}
                dy="-1em"
                textAnchor="bottom"
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
